using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool hasMoved;

    public override List<Vector2Int> GetAllValidMoves(Board board)
    {
        List<Vector2Int> validMoves = base.GetAllValidMoves(board);
        validMoves.AddRange(GetAttackMoves(board)); 
        return validMoves;
    }

    public override List<Vector2Int> GetAttackMoves(Board board)
    {
        List<Vector2Int> attackSquares = new();

        // Pawn can take enemy's piece in left and right forward position
        Vector2Int newMove = Color == TeamColor.White ? new(1, 1) : new(1, -1);
        if (IsInsideBoard(BoardIndex + newMove) &&
            board.GetPiece(BoardIndex + newMove) != null &&
            board.GetPiece(BoardIndex + newMove).Color != this.Color)
        {
            attackSquares.Add(newMove);
        }

        newMove = Color == TeamColor.White ? new(-1, 1) : new(-1, -1);
        if (IsInsideBoard(BoardIndex + newMove) &&
            board.GetPiece(BoardIndex + newMove) != null &&
            board.GetPiece(BoardIndex + newMove).Color != this.Color)
        {
            attackSquares.Add(newMove);
        }

        return attackSquares;
    }

    protected override List<Vector2Int> GetSpecialMoves(Board board)
    {
        List<Vector2Int> specialMoves = base.GetSpecialMoves(board);

        Vector2Int newMove = Color == TeamColor.White ? new(0, 2) : new(0, -2);

        // Pawn can move 2 cell formard in the first move
        if (!hasMoved && IsInsideBoard(BoardIndex + newMove))
        {
            specialMoves.Add(newMove);
        }

        return specialMoves;
    }

    public override void MoveTo(Vector2Int newPos)
    {
        base.MoveTo(newPos);
        hasMoved = true;
    }
}
