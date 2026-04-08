using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public bool hasMoved;

    public override void MoveTo(Vector2Int newPos)
    {
        base.MoveTo(newPos);
        hasMoved = true;
    }
}
