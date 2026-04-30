using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool hasMoved;

    public override List<Move> GetLegalMoves(Board board)
    {
        List<Move> legalMoves = new();

        int forward = Color == TeamColor.White ? 1 : -1;
        Vector2Int oneStep = BoardIndex + new Vector2Int(0, forward);
        if (IsInsideBoard(oneStep) && board.GetPiece(oneStep) == null)
        {
            legalMoves.Add(new Move(BoardIndex, oneStep, MoveType.Normal));

            Vector2Int twoStep = BoardIndex + new Vector2Int(0, forward * 2);
            if (!hasMoved && IsInsideBoard(twoStep) && board.GetPiece(twoStep) == null)
            {
                legalMoves.Add(new Move(BoardIndex, twoStep, MoveType.Normal));
            }
        }

        TryAddDiagonalCapture(board, legalMoves, new Vector2Int(1, forward));
        TryAddDiagonalCapture(board, legalMoves, new Vector2Int(-1, forward));
        TryAddEnPassant(board, legalMoves, forward);

        return legalMoves;
    }

    public override List<Vector2Int> GetAttackSquares(Board board)
    {
        List<Vector2Int> attacks = new();
        int forward = Color == TeamColor.White ? 1 : -1;

        Vector2Int right = BoardIndex + new Vector2Int(1, forward);
        Vector2Int left = BoardIndex + new Vector2Int(-1, forward);

        if (IsInsideBoard(right)) attacks.Add(right);
        if (IsInsideBoard(left)) attacks.Add(left);

        return attacks;
    }

    private void TryAddDiagonalCapture(Board board, List<Move> legalMoves, Vector2Int diagonal)
    {
        Vector2Int target = BoardIndex + diagonal;
        if (!IsInsideBoard(target)) return;

        ChessPiece piece = board.GetPiece(target);
        if (piece != null && piece.Color != Color)
        {
            legalMoves.Add(new Move(BoardIndex, target, MoveType.Capture));
        }
    }

    private void TryAddEnPassant(Board board, List<Move> legalMoves, int forward)
    {
        Move lastMove = GameManager.Instance.LastMove;
        if (lastMove == null) return;

        ChessPiece movedPiece = board.GetPiece(lastMove.To);
        if (movedPiece == null || movedPiece.PieceSO.type != PieceType.Pawn || movedPiece.Color == Color)
        {
            return;
        }

        bool isTwoStepPawnMove = Mathf.Abs(lastMove.To.y - lastMove.From.y) == 2;
        if (!isTwoStepPawnMove || lastMove.To.y != BoardIndex.y) return;

        if (Mathf.Abs(lastMove.To.x - BoardIndex.x) != 1) return;

        Vector2Int destination = new Vector2Int(lastMove.To.x, BoardIndex.y + forward);
        if (!IsInsideBoard(destination) || board.GetPiece(destination) != null) return;

        Move enPassant = new Move(BoardIndex, destination, MoveType.EnPassant)
        {
            CapturedSquare = lastMove.To
        };

        legalMoves.Add(enPassant);
    }
}
