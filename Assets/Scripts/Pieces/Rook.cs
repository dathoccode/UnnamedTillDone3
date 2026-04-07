using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public bool hasMoved;

    public override  List<Vector2Int> GetAllValidMoves(Board board)
    {
        List<Vector2Int> validMoves = base.GetAllValidMoves(board);

        return validMoves;
    }

    public override void MoveTo(Vector2Int newPos)
    {
        base.MoveTo(newPos);
        hasMoved = true;
    }
}
