using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChessPiece : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public PieceSO PieceSO;
    public TeamColor Color;
    public Vector2Int BoardIndex;
    public List<Move> LegalMoves { get; private set; } = new List<Move>();

    [SerializeField] private GameManager gameManager;

    public ChessPiece InitailizePiece(PieceType type, TeamColor color)
    {
        
        PieceSO = Resources.Load<PieceSO>("PieceData/" + type.ToString());
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        Color = color;
        spriteRenderer.sprite = color == TeamColor.White ? PieceSO.whiteSprite : PieceSO.blackSprite;

        BoardIndex = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        return this;
    }

    public virtual void ApplyMove(Move move)
    {
        BoardIndex = move.To;
        transform.position = new Vector2(move.To.x, move.To.y);
    }

    public void RecoverPosition() 
    {
        transform.position = new Vector2(BoardIndex.x, BoardIndex.y);
    }

    public virtual List<Move> GetLegalMoves()
    { 
        LegalMoves.Clear();
        foreach (var pattern in PieceSO.movePatterns)
        {
            Vector2Int tempPattern = pattern;
            if (Color == TeamColor.Black) tempPattern = new(pattern.x * -1, pattern.y * -1);

            Move newMove = new(BoardIndex, BoardIndex + tempPattern, MoveType.Normal);

            while (IsInsideBoard(newMove.To))
            {
                if (Board.Instance.GetPiece(newMove.To) == null) LegalMoves.Add(newMove);
                else
                {
                    if (Board.Instance.GetPiece(newMove.To).Color != Color)
                    {
                        LegalMoves.Add(newMove);
                    }
                    break;
                }

                if (!PieceSO.isSliding) break;

                newMove = new (BoardIndex, newMove.To + tempPattern, MoveType.Normal);
            }
        }
        return LegalMoves;
    }

    public virtual List<Move> GetAttackMoves()
    {
        List<Move> AttackMoves = new();
        foreach (var move in LegalMoves)
        {
            if (move.Type == MoveType.Capture || move.Type == MoveType.Normal) AttackMoves.Add(move);

        }
        return AttackMoves;
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
