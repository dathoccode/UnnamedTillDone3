using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class ChessPiece : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public PieceSO PieceSO;
    public TeamColor Color;
    public Vector2Int BoardIndex;

    [SerializeField] private GameManager gameManager;

    public ChessPiece InitailizePiece(PieceType type, TeamColor color)
    {
        
        this.PieceSO = Resources.Load<PieceSO>("PieceData/" + type.ToString());
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        Color = color;
        spriteRenderer.sprite = color == TeamColor.White ? PieceSO.whiteSprite : PieceSO.blackSprite;

        BoardIndex = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.y);
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

    public virtual List<Vector2Int> GetAllValidMove(Board board) 
    {
        List<Vector2Int> validMoves = new();
        foreach (var pattern in PieceSO.movePatterns)
        {
            // Reverse pattern for black piece
            Vector2Int tempPattern = pattern;
            if (Color == TeamColor.Black)
            {
                tempPattern = new(pattern.x * -1, pattern.y * -1);
            }

            Vector2Int pos = BoardIndex + tempPattern;

            while(IsInsideBoard(pos))
            {
                if (!board.GetPiece(pos) || board.GetPiece(pos).Color != this.Color)
                {
                    validMoves.Add(tempPattern);
                }
                
                // There's an ally piece on the way
                if (board.GetPiece(pos) && board.GetPiece(pos).Color == this.Color)
                {
                    break;
                }

                // This piece can't slide
                if (!this.PieceSO.isSliding)
                {
                    break;
                }

                // Translate pos by tempPattern when piece can slide
                if(Color == TeamColor.White)
                {
                    tempPattern += pattern;
                    pos += pattern;
                }
                else
                {
                    tempPattern -= pattern;
                    pos -= pattern;
                }

            }

        }

      
        return validMoves;
    }

    private Vector2Int NormalizeDir(Vector2Int dir)
    {
        int gcd = GCD(dir.x, dir.y);
        int x = dir.x / Mathf.Abs(gcd);
        int y = dir.y / Mathf.Abs(gcd);

        return new Vector2Int(x, y);
    }

    int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
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
