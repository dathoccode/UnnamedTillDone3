using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool hasMoved;

    public override  List<Vector2Int> GetAllValidMove(ChessPiece[,] board)
    {
        List<Vector2Int> validMoves = base.GetAllValidMove(board);

        // Pawn can move 2 cell formard in the first move

        if (!hasMoved)
        {
            validMoves.Add(new Vector2Int(BoardIndex.x, BoardIndex.y + 2));
        }

        // Pawn can take enemy's piece in left and right forward position
        if (IsInsideBoard(new Vector2Int(BoardIndex.x + 1, BoardIndex.y + 1)) && 
            board[BoardIndex.x + 1, BoardIndex.y + 1] != null &&
            board[BoardIndex.x + 1, BoardIndex.y + 1].Color != this.Color)
        {
            validMoves.Add(new Vector2Int(1, 1));
        }
        if (IsInsideBoard(new Vector2Int(BoardIndex.x - 1, BoardIndex.y + 1)) &&
            board[BoardIndex.x - 1, BoardIndex.y + 1] != null &&
            board[BoardIndex.x - 1, BoardIndex.y + 1].Color != this.Color)
        {
            validMoves.Add(new Vector2Int(-1, 1));
        }

        return validMoves;
    }
}
