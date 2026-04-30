using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool hasMoved;

    public override List<Vector2Int> GetPatternMoves(Board board)
    {
        List<Vector2Int> moves = new();

        int direction = Color == TeamColor.White ? 1 : -1;
        Vector2Int forwardOne = BoardIndex + new Vector2Int(0, direction);

        if (IsInsideBoard(forwardOne) && board.GetPiece(forwardOne) == null)
        {
            moves.Add(new Vector2Int(0, direction));
        }

        AddCaptureMove(board, moves, new Vector2Int(1, direction));
        AddCaptureMove(board, moves, new Vector2Int(-1, direction));

        return moves;
    }

    public override List<Vector2Int> GetAttackMoves(Board board)
    {
        List<Vector2Int> attacks = new();
        int direction = Color == TeamColor.White ? 1 : -1;

        if (IsInsideBoard(BoardIndex + new Vector2Int(1, direction))) attacks.Add(new Vector2Int(1, direction));
        if (IsInsideBoard(BoardIndex + new Vector2Int(-1, direction))) attacks.Add(new Vector2Int(-1, direction));

        return attacks;
    }

    public override List<Vector2Int> GetSpecialMoves(Board board)
    {
        List<Vector2Int> specialMoves = new();

        int direction = Color == TeamColor.White ? 1 : -1;
        Vector2Int forwardOne = BoardIndex + new Vector2Int(0, direction);
        Vector2Int forwardTwo = BoardIndex + new Vector2Int(0, 2 * direction);

        if (!hasMoved && IsInsideBoard(forwardTwo) && board.GetPiece(forwardOne) == null && board.GetPiece(forwardTwo) == null)
        {
            specialMoves.Add(new Vector2Int(0, 2 * direction));
        }

        if (board.CurrentEnPassant.HasValue)
        {
            var enPassant = board.CurrentEnPassant.Value;
            if (enPassant.CaptureSquare.y == BoardIndex.y + direction && Mathf.Abs(enPassant.CaptureSquare.x - BoardIndex.x) == 1)
            {
                specialMoves.Add(enPassant.CaptureSquare - BoardIndex);
            }
        }

        return specialMoves;
    }

    public override void MoveTo(Vector2Int newPos)
    {
        base.MoveTo(newPos);
        hasMoved = true;
    }

    private void AddCaptureMove(Board board, List<Vector2Int> moves, Vector2Int offset)
    {
        Vector2Int targetPos = BoardIndex + offset;
        if (!IsInsideBoard(targetPos)) return;

        ChessPiece target = board.GetPiece(targetPos);
        if (target != null && target.Color != Color)
        {
            moves.Add(offset);
        }
    }
}
