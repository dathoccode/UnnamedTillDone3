using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool hasMoved;

    public override  List<Vector2Int> GetAllValidMove(Board board)
    {
        List<Vector2Int> validMoves = base.GetAllValidMove(board);

        Vector2Int newMove = Color == TeamColor.White ? new(0, 2) : new(0, -2);

        // Pawn can move 2 cell formard in the first move
        if (!hasMoved)
        {
            validMoves.Add(newMove);
        }

        // Pawn can take enemy's piece in left and right forward position
        newMove = Color == TeamColor.White ? new(1, 1) : new(1, -1);
        if (IsInsideBoard(BoardIndex + newMove) && 
            board.GetPiece(BoardIndex + newMove) != null &&
            board.GetPiece(BoardIndex + newMove).Color != this.Color)
        {
            validMoves.Add(newMove);
        }

        newMove = Color == TeamColor.White ? new(-1, 1) : new(-1, -1);
        if (IsInsideBoard(BoardIndex + newMove) &&
            board.GetPiece(BoardIndex + newMove) != null &&
            board.GetPiece(BoardIndex + newMove).Color != this.Color)
        {
            validMoves.Add(newMove);
        }

        return validMoves;
    }
}
