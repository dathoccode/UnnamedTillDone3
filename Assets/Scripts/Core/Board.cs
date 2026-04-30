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

    private bool[,] whiteAttackMap = new bool[8, 8], blackAttackMap = new bool[8, 8];

    public EnPassantInfo? CurrentEnPassant { get; private set; }

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
                CreatePiece(piece.Key, position, TeamColor.White);
                CreatePiece(piece.Key, new Vector2Int(position.x, 7 - position.y), TeamColor.Black);
            }
        }
    }

    public ChessPiece CreatePiece(PieceType type, Vector2Int pos, TeamColor color)
    {
        GameObject obj = Instantiate(piecePrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);

        ChessPiece piece = AddPieceComponent(type, obj);
        piece.InitailizePiece(type, color);

        piece.transform.SetParent(holder.transform, false);
        piece.name = $"{color}_{type}_{pos.x}_{pos.y}";

        grid[pos.x, pos.y] = piece;
        return piece;
    }

    public void PromotePawn(Pawn pawn, PieceType promoteTo = PieceType.Queen)
    {
        Vector2Int pos = pawn.BoardIndex;
        TeamColor color = pawn.Color;

        Destroy(pawn.gameObject);
        CreatePiece(promoteTo, pos, color);
    }

    public ChessPiece GetPiece(Vector2Int pos) => grid[pos.x, pos.y];
    public ChessPiece GetPiece(int x, int y) => grid[x, y];

    private ChessPiece AddPieceComponent(PieceType type, GameObject prefab)
    {
        return type switch
        {
            PieceType.Pawn => prefab.AddComponent<Pawn>(),
            PieceType.Rook => prefab.AddComponent<Rook>(),
            PieceType.King => prefab.AddComponent<King>(),
            PieceType.Bishop => prefab.AddComponent<Bishop>(),
            PieceType.Knight => prefab.AddComponent<Knight>(),
            PieceType.Queen => prefab.AddComponent<Queen>(),
            _ => prefab.AddComponent<Queen>()
        };
    }

    public bool TryMovePiece(ChessPiece movingPiece, Vector2Int to, out MoveModel move)
    {
        move = BuildMoveModel(movingPiece, to);
        EnPassantInfo? prevEnPassant = CurrentEnPassant;

        if (move.IsEnPassant)
        {
            grid[move.CapturedPosition.x, move.CapturedPosition.y] = null;
        }

        grid[move.To.x, move.To.y] = movingPiece;
        grid[move.From.x, move.From.y] = null;

        Vector2Int originalIndex = movingPiece.BoardIndex;
        movingPiece.BoardIndex = move.To;

        if (IsKingChecked(movingPiece.Color))
        {
            RollbackMove(movingPiece, move);
            CurrentEnPassant = prevEnPassant;
            return false;
        }

        movingPiece.BoardIndex = originalIndex;

        if (move.CapturedPiece != null)
        {
            Destroy(move.CapturedPiece.gameObject);
        }

        move.IsPromotion = movingPiece is Pawn && (move.To.y == 0 || move.To.y == 7);
        UpdateEnPassantState(movingPiece, move.From, move.To);
        return true;
    }

    private MoveModel BuildMoveModel(ChessPiece movingPiece, Vector2Int to)
    {
        Vector2Int from = movingPiece.BoardIndex;
        bool isEnPassant = movingPiece is Pawn &&
                           CurrentEnPassant.HasValue &&
                           CurrentEnPassant.Value.CaptureSquare == to;

        Vector2Int capturedPos = isEnPassant ? CurrentEnPassant.Value.PawnPosition : to;

        return new MoveModel
        {
            Piece = movingPiece,
            From = from,
            To = to,
            IsEnPassant = isEnPassant,
            CapturedPosition = capturedPos,
            CapturedPiece = GetPiece(capturedPos),
            IsPromotion = false
        };
    }

    private void RollbackMove(ChessPiece movingPiece, MoveModel move)
    {
        movingPiece.BoardIndex = move.From;
        grid[move.From.x, move.From.y] = movingPiece;
        grid[move.To.x, move.To.y] = null;

        if (move.CapturedPiece != null)
        {
            grid[move.CapturedPosition.x, move.CapturedPosition.y] = move.CapturedPiece;
        }
    }

    private void UpdateEnPassantState(ChessPiece movingPiece, Vector2Int from, Vector2Int to)
    {
        CurrentEnPassant = null;

        if (movingPiece is not Pawn) return;

        if (Mathf.Abs(to.y - from.y) == 2)
        {
            int direction = movingPiece.Color == TeamColor.White ? 1 : -1;
            CurrentEnPassant = new EnPassantInfo
            {
                PawnPosition = to,
                CaptureSquare = new Vector2Int(to.x, to.y - direction)
            };
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
            foreach (var move in attackMoves)
            {
                Vector2Int pos = piece.BoardIndex + move;
                if (piece.Color == TeamColor.White) whiteAttackMap[pos.x, pos.y] = true;
                else blackAttackMap[pos.x, pos.y] = true;
            }
        }
    }

    public bool IsSquareAttacked(Vector2Int pos, TeamColor defendingColor)
    {
        BuildAttackMap();
        return defendingColor == TeamColor.White ? blackAttackMap[pos.x, pos.y] : whiteAttackMap[pos.x, pos.y];
    }

    private Vector2Int GetKingPosition(TeamColor color)
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

    private bool IsKingChecked(TeamColor color)
    {
        Vector2Int kingPos = GetKingPosition(color);
        return IsSquareAttacked(kingPos, color);
    }
}

public struct EnPassantInfo
{
    public Vector2Int PawnPosition;
    public Vector2Int CaptureSquare;
}
