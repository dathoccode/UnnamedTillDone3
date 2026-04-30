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

    public virtual void MoveTo(Vector2Int newPos)
    {
        BoardIndex = newPos;
        transform.position = new Vector2(newPos.x, newPos.y);
    }

    public void RecoverPosition()
    {
        transform.position = new Vector2(BoardIndex.x, BoardIndex.y);
    }

    public virtual List<Vector2Int> GetPatternMoves(Board board)
    {
        List<Vector2Int> patternMoves = new();

        foreach (var pattern in PieceSO.movePatterns)
        {
            Vector2Int step = Color == TeamColor.Black ? new Vector2Int(-pattern.x, -pattern.y) : pattern;
            Vector2Int currentOffset = step;
            Vector2Int pos = BoardIndex + step;

            while (IsInsideBoard(pos))
            {
                ChessPiece target = board.GetPiece(pos);
                if (target == null)
                {
                    patternMoves.Add(currentOffset);
                }
                else
                {
                    if (target.Color != Color) patternMoves.Add(currentOffset);
                    break;
                }

                if (!PieceSO.isSliding) break;

                currentOffset += step;
                pos += step;
            }
        }

        return patternMoves;
    }

    public virtual List<Vector2Int> GetAttackMoves(Board board) => GetPatternMoves(board);

    public virtual List<Vector2Int> GetSpecialMoves(Board board) => new();

    public virtual List<Vector2Int> GetValidMoves(Board board)
    {
        var validMoves = GetPatternMoves(board);
        validMoves.AddRange(GetSpecialMoves(board));

        validMoves.RemoveAll(move =>
        {
            Vector2Int newPos = BoardIndex + move;
            if (!IsInsideBoard(newPos)) return true;

            var targetPiece = board.GetPiece(newPos);
            return targetPiece != null && targetPiece.Color == Color;
        });

        return validMoves;
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
