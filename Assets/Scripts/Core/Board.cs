using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public ChessPiece[,] grid = new ChessPiece[8, 8];
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject holder;

    private void Awake()
    {
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
        GameObject obj = Instantiate(piecePrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);

        ChessPiece piece = AddPieceComponent(type, obj);
        piece.InitailizePiece(type, color);

        piece.transform.SetParent(holder.transform, false);
        piece.name = $"{color}_{type}_{pos.x}_{pos.y}";

        grid[pos.x, pos.y] = piece;
    }

    public void ApplyMove(Move move)
    {
        ChessPiece movingPiece = grid[move.From.x, move.From.y];
        if (movingPiece == null) return;

        if (move.Type == MoveType.EnPassant)
        {
            RemovePieceAt(move.CapturedSquare);
        }
        else if (grid[move.To.x, move.To.y] != null)
        {
            RemovePieceAt(move.To);
        }

        grid[move.To.x, move.To.y] = movingPiece;
        grid[move.From.x, move.From.y] = null;

        movingPiece.MoveTo(move.To);

        if (movingPiece is Pawn pawn) pawn.hasMoved = true;
        if (movingPiece is King king) king.hasMoved = true;
        if (movingPiece is Rook rook) rook.hasMoved = true;
    }

    private void RemovePieceAt(Vector2Int pos)
    {
        if (grid[pos.x, pos.y] == null) return;
        Destroy(grid[pos.x, pos.y].gameObject);
        grid[pos.x, pos.y] = null;
    }


    public bool[,] BuildAttackMap(TeamColor attackerColor)
    {
        bool[,] attackMap = new bool[8, 8];

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = grid[x, y];
                if (piece == null || piece.Color != attackerColor) continue;

                foreach (Vector2Int square in piece.GetAttackSquares(this))
                {
                    attackMap[square.x, square.y] = true;
                }
            }
        }

        return attackMap;
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
}
