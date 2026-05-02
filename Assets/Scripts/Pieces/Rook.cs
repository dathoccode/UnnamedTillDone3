using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public bool HasMove { private set; get; }

    public override void ApplyMove(Move move)
    {
        base.ApplyMove(move);
        HasMove = true;
    }
}
