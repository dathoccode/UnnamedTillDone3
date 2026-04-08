using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public bool HasMove { private set; get; }

    public void HandleCastlingRookMove()
    {
        // kingside
        if (BoardIndex.x == 7)
        {
            MoveTo(new Vector2Int(5, BoardIndex.y));
        }
        // queenside
        else if (BoardIndex.x == 0)
        {
            MoveTo(new Vector2Int(3, BoardIndex.y));
        }
    }

    public override void MoveTo(Vector2Int newPos)
    {
        base.MoveTo(newPos);
        HasMove = true;
    }
}
