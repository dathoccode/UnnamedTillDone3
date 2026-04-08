using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool hasMoved;

    // Depreciated
    public override List<Vector2Int> GetPatternMoves(Board board)
    {
        List<Vector2Int> patternMoves = base.GetPatternMoves(board);

        // Pawn can move to left and right forward position if there is an opponent piece
        Vector2Int newMove = Color == TeamColor.White ? new(1, 1) : new(1, -1);
        if (IsInsideBoard(BoardIndex + newMove) &&
            board.GetPiece(BoardIndex + newMove) != null &&
            board.GetPiece(BoardIndex + newMove).Color != this.Color)
        {
            patternMoves.Add(newMove);
        }

        newMove = Color == TeamColor.White ? new(-1, 1) : new(-1, -1);
        if (IsInsideBoard(BoardIndex + newMove) &&
            board.GetPiece(BoardIndex + newMove) != null &&
            board.GetPiece(BoardIndex + newMove).Color != this.Color)
        {
            patternMoves.Add(newMove);
        }

        return patternMoves;
    }

    public override List<Vector2Int> GetAttackMoves(Board board)
    {
        List<Vector2Int> specialMoves = base.GetAttackMoves(board);
        if (Color == TeamColor.White)
        {
            if (IsInsideBoard(BoardIndex + new Vector2Int(1, 1)))
            {
                specialMoves.Add(new Vector2Int(1, 1));
            }
            if (IsInsideBoard(BoardIndex + new Vector2Int(-1, 1)))
            {
                specialMoves.Add(new Vector2Int(-1, 1));
            }
        }
        else
        {
            if (IsInsideBoard(BoardIndex + new Vector2Int(1, -1)))
            {
                specialMoves.Add(new Vector2Int(1, -1));
            }
            if (IsInsideBoard(BoardIndex + new Vector2Int(-1, -1)))
            {
                specialMoves.Add(new Vector2Int(-1, -1));
            }
        }

        return specialMoves;
    }


    public override List<Vector2Int> GetSpecialMoves(Board board)
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
