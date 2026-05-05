using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    private static Board instance;
    public static Board Instance => instance;

    public ChessPiece[,] grid = new ChessPiece[8, 8];
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject holder;

    private bool[,] whiteAttackMap = new bool[8, 8], blackAttackMap = new bool[8,8];

    private void Awake()
    {
        if (instance == null) instance = this;
        tilemap = GetComponent<Tilemap>();
    }

    public void InitPieces()
    {
        foreach (var piece in Data.WhiteStartPositions)
        {
            foreach (var position in piece.Value)
            {
                SpawnPiece(piece.Key, position, TeamColor.White);
                SpawnPiece(piece.Key, new Vector2Int(position.x, 7 - position.y), TeamColor.Black);
            }
        }
    }

    private void SpawnPiece(PieceType type, Vector2Int pos, TeamColor color)
    {
        GameObject obj = Instantiate(piecePrefab,
            new Vector3(pos.x, pos.y, 0),
            Quaternion.identity);

        ChessPiece piece = AddPieceComponent(type, obj);
        piece.InitailizePiece(type, color);

        piece.transform.SetParent(holder.transform, false);
        piece.name = $"{color}_{type}_{pos.x}_{pos.y}";

        grid[pos.x, pos.y] = piece;
    }

    public ChessPiece GetPiece(Vector2Int pos)
    {
        return grid[pos.x, pos.y];
    }

    public ChessPiece GetPiece(int x, int y)
    {
        return grid[x, y];
    }

    private ChessPiece AddPieceComponent(PieceType type, GameObject prefab)
    {
        switch (type)
        {
            case PieceType.Pawn:
                return prefab.AddComponent<Pawn>();
            case PieceType.Rook:
                return prefab.AddComponent<Rook>();
            case PieceType.King:
                return prefab.AddComponent<King>();
            default:
                return prefab.AddComponent<Queen>();
        }
    }

    private void ClearAttackMap()
    {
        Array.Clear(whiteAttackMap, 0, whiteAttackMap.Length);
        Array.Clear(blackAttackMap, 0, blackAttackMap.Length);
    }

    private void BuildAttackMap()
    {
        ClearAttackMap();

        foreach (var piece in grid)
        {
            if (piece == null) continue;
            List<Move> attackMoves = piece.GetAttackMoves();
            foreach (var move in attackMoves)
            {
                if (piece.Color == TeamColor.White)
                {
                    whiteAttackMap[move.CapturedSquare.x, move.CapturedSquare.y] = true;
                }
                else
                {
                    blackAttackMap[move.CapturedSquare.x, move.CapturedSquare.y] = true;
                }
            }
        }
    }

    public bool IsSquareAttacked(Vector2Int pos, TeamColor byColor)
    {
        if (byColor == TeamColor.White) return blackAttackMap[pos.x, pos.y];
        return whiteAttackMap[pos.x, pos.y];
    }
    
    public bool ApplyMove(Move move)
    {
        ChessPiece movingPiece = GetPiece(move.From);
        ChessPiece capturedPiece = GetPiece(move.CapturedSquare);

        movingPiece.BoardIndex = move.To;
        grid[move.CapturedSquare.x, move.CapturedSquare.y] = null;
        grid[move.To.x, move.To.y] = movingPiece;
        grid[move.From.x, move.From.y] = null;
        BuildAttackMap();

        if (IsKingChecked())
        {
            grid[move.From.x, move.From.y] = movingPiece;
            grid[move.CapturedSquare.x, move.CapturedSquare.y] = capturedPiece;
            movingPiece.BoardIndex = move.From;
            BuildAttackMap();
            Debug.Log("Board: Cant move because king is checked");
            return false;
        }

        movingPiece.ApplyMove(move);

        if (capturedPiece != null) Destroy(capturedPiece.gameObject);

        return true;
    }

    Vector2Int GetKingPosition(TeamColor color)
    {
        foreach (var piece in grid)
        {
            if (piece != null && piece.PieceSO.type == PieceType.King && piece.Color == color)
            {
                return piece.BoardIndex;
            }
        }
        return new Vector2Int(-1, -1);
    }

    private bool IsKingChecked()
    {
        Vector2Int kingPos = GetKingPosition(GameManager.Instance.curTurn);

        return IsSquareAttacked(kingPos, GameManager.Instance.curTurn);
    }

    public void PromotePawn(Move move, PieceType type)
    {
        Pawn promotedPawn = GetPiece(move.To) as Pawn;
        if (promotedPawn == null || type == PieceType.Pawn || type == PieceType.King) return;

        Vector2Int pawnPosition = promotedPawn.BoardIndex;

        grid[pawnPosition.x, pawnPosition.y] = null;
        Destroy(promotedPawn.gameObject);

        SpawnPiece(type, pawnPosition, promotedPawn.Color);    
    }

    private void ShowMatrix(bool[,] attackMap)
    {
        string output = "";

        for (int i = 7; i >= 0; i--)
        {
            for (int j = 0; j < 8; j++)
            {
                output += (attackMap[j, i] ? "2 " : "0 ");
            }
            output += "\n";
        }

        Debug.Log(output);
    }
}
