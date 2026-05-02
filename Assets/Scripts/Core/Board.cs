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
        whiteAttackMap = new bool[8, 8];
        blackAttackMap = new bool[8, 8];
    }

    private void BuildAttackMap()
    {
        ClearAttackMap();

        foreach (var piece in grid)
        {
            if (piece == null) continue;
            var attackMoves = piece.GetAttackMoves();
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
        BuildAttackMap();
        if (byColor == TeamColor.White) return blackAttackMap[pos.x, pos.y];
        return whiteAttackMap[pos.x, pos.y];
    }
    
    public bool ApplyMove(Move move)
    {
        ChessPiece movingPiece = GetPiece(move.From);
        ChessPiece capturedPiece = GetPiece(move.CapturedSquare);

        grid[move.CapturedSquare.x, move.CapturedSquare.y] = null;
        grid[move.To.x, move.To.y] = movingPiece;
        grid[move.From.x, move.From.y] = null;

        if (IsKingChecked())
        {
            grid[move.From.x, move.From.y] = movingPiece;
            grid[move.CapturedSquare.x, move.CapturedSquare.y] = capturedPiece;
            Debug.Log("Board: Cant move because king is checked");
            return false;
        } 

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
}
