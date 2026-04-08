using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public ChessPiece[,] grid = new ChessPiece[8, 8];
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject holder;

    private bool[,] whiteAttackMap = new bool[8, 8], blackAttackMap = new bool[8,8];

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
        whiteAttackMap = new bool[8, 8];
        blackAttackMap = new bool[8, 8];
    }

    private void BuildAttackMap()
    {
        ClearAttackMap();

        foreach (var piece in grid)
        {
            if (piece == null) continue;
            var attackMoves = piece.GetAttackMoves(this);
            Debug.Log($"Building Attack map with: {piece.name} and there are {attackMoves.Count} attack moves");
            foreach (var move in attackMoves)
            {
                Vector2Int pos = piece.BoardIndex + move;
                if (piece.Color == TeamColor.White)
                {
                    whiteAttackMap[pos.x, pos.y] = true;
                    Debug.Log($"White attacks {pos}");
                }
                else
                {
                    blackAttackMap[pos.x, pos.y] = true;
                    Debug.Log($"Black attacks {pos}"); 
                }
            }
        }
    }

    public bool OnPieceMove(Vector2Int from, Vector2Int to)
    {
        // backu
        ChessPiece backupPiece = GetPiece(to);
        grid[to.x, to.y] = grid[from.x, from.y];
        grid[from.x, from.y] = null;

        BuildAttackMap();

        if (IsKingChecked())
        {
            grid[from.x, from.y] = grid[to.x, to.y];
            grid[to.x, to.y] = backupPiece;
            return false;
        }

        if (backupPiece != null) Destroy(backupPiece.gameObject);


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
        if (GameManager.Instance.curTurn == TeamColor.White && blackAttackMap[kingPos.x, kingPos.y])
        {
            Debug.Log("White King is in check!");
            return true;
        }
        if (GameManager.Instance.curTurn == TeamColor.Black && whiteAttackMap[kingPos.x, kingPos.y])
        {
           
            Debug.Log("Black King is in check!");
            return true;
        }
        return false;
    }
}
