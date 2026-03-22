using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public bool hasMoved;

    public override  List<Vector2Int> GetAllValidMove(ChessPiece[,] board)
    {
        List<Vector2Int> validMoves = base.GetAllValidMove(board);

        return validMoves;
    }
}
