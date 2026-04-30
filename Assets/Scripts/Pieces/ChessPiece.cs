using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChessPiece : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public PieceSO PieceSO;
    public TeamColor Color;
    public Vector2Int BoardIndex;

    public ChessPiece InitailizePiece(PieceType type, TeamColor color)
    {
        PieceSO = Resources.Load<PieceSO>("PieceData/" + type);
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        Color = color;
        spriteRenderer.sprite = color == TeamColor.White ? PieceSO.whiteSprite : PieceSO.blackSprite;
        BoardIndex = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        return this;
    }

    public void MoveTo(Vector2Int newPos)
    {
        BoardIndex = newPos;
        transform.position = new Vector2(newPos.x, newPos.y);
    }

    public void RecoverPosition()
    {
        transform.position = new Vector2(BoardIndex.x, BoardIndex.y);
    }

    public virtual List<Move> GetLegalMoves(Board board)
    {
        List<Move> legalMoves = new();

        foreach (var pattern in PieceSO.movePatterns)
        {
            Vector2Int normalizedPattern = Color == TeamColor.Black
                ? new Vector2Int(pattern.x * -1, pattern.y * -1)
                : pattern;

            Vector2Int offset = normalizedPattern;
            Vector2Int target = BoardIndex + offset;

            while (IsInsideBoard(target))
            {
                ChessPiece occupiedPiece = board.GetPiece(target);

                if (occupiedPiece == null)
                {
                    legalMoves.Add(new Move(BoardIndex, target, MoveType.Normal));
                }
                else
                {
                    if (occupiedPiece.Color != Color)
                    {
                        legalMoves.Add(new Move(BoardIndex, target, MoveType.Capture));
                    }

                    break;
                }

                if (!PieceSO.isSliding)
                {
                    break;
                }

                offset += normalizedPattern;
                target = BoardIndex + offset;
            }
        }

        return legalMoves;
    }


    public virtual List<Vector2Int> GetAttackSquares(Board board)
    {
        List<Vector2Int> attacks = new();
        foreach (Move move in GetLegalMoves(board))
        {
            if (move.Type == MoveType.Capture || move.Type == MoveType.EnPassant || move.Type == MoveType.Castling)
            {
                attacks.Add(move.To);
            }
        }

        return attacks;
    }

    protected bool IsInsideBoard(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }

    public void EnableFollowMouse()
    {
        GetComponent<FollowMouse>().enabled = true;
    }
}
