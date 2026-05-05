using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool hasMoved;

    public override List<Move> GetLegalMoves()
    {
        Move patternMove = new(BoardIndex, Vector2Int.zero, MoveType.Normal);
        patternMove.To = Color == TeamColor.White ? BoardIndex + new Vector2Int(0, 1) : BoardIndex + new Vector2Int(0, -1);
        if (IsInsideBoard(patternMove.To) && Board.Instance.GetPiece(patternMove.To) == null)
        {
            if (Color == TeamColor.White && patternMove.To.y == 7 ||
                Color == TeamColor.Black && patternMove.To.y == 0)
            {
                patternMove.Type = MoveType.Promotion;
            }
            patternMove.CapturedSquare = patternMove.To;
            LegalMoves.Add(patternMove);
        }

        // Capture move
        Move captureMove = new(BoardIndex, Vector2Int.zero, MoveType.Capture);
        captureMove.To = Color == TeamColor.White ? BoardIndex + new Vector2Int(1, 1) : BoardIndex + new Vector2Int(1, -1);
        captureMove.CapturedSquare = captureMove.To;
        if (IsInsideBoard(captureMove.To) &&
            Board.Instance.GetPiece(captureMove.To) != null &&
            Board.Instance.GetPiece(captureMove.To).Color != Color)
        {
            LegalMoves.Add(captureMove);
        }

        captureMove = new(BoardIndex, Vector2Int.zero, MoveType.Capture);
        captureMove.To = Color == TeamColor.White ? BoardIndex + new Vector2Int(-1, 1) : BoardIndex + new Vector2Int(-1, -1);
        captureMove.CapturedSquare = captureMove.To;
        if (IsInsideBoard(captureMove.To) &&
            Board.Instance.GetPiece(captureMove.To) != null &&
            Board.Instance.GetPiece(captureMove.To).Color != Color)
        {
            LegalMoves.Add(captureMove);
        }

        // Initialize move
        Move firstMove = new(BoardIndex, Vector2Int.zero, MoveType.Normal);
        firstMove.To = Color == TeamColor.White ? BoardIndex + new Vector2Int(0, 2) : BoardIndex + new Vector2Int(0, -2);
        firstMove.CapturedSquare = firstMove.To;
        Vector2Int midleSqr = (firstMove.To - BoardIndex) / 2;
        
        if (IsInsideBoard(firstMove.To) &&
            hasMoved == false &&
            Board.Instance.GetPiece(firstMove.To) == null &&
            Board.Instance.GetPiece(BoardIndex + midleSqr) == null)
        {
            LegalMoves.Add(firstMove);
        }

        // En passant move
        TryEnPassant();

        return LegalMoves;
    }

    public override List<Move> GetAttackMoves()
    {
        List<Move> AttackMoves = new();

        Move captureMove = new(BoardIndex, Vector2Int.zero, MoveType.Capture);
        captureMove.To = Color == TeamColor.White ? BoardIndex + new Vector2Int(1, 1) : BoardIndex + new Vector2Int(1, -1);
        captureMove.CapturedSquare = captureMove.To;
        if (IsInsideBoard(captureMove.To)) AttackMoves.Add(captureMove);

        captureMove = new(BoardIndex, Vector2Int.zero, MoveType.Capture);
        captureMove.To = Color == TeamColor.White ? BoardIndex + new Vector2Int(-1, 1) : BoardIndex + new Vector2Int(-1, -1);
        captureMove.CapturedSquare = captureMove.To;
        if (IsInsideBoard(captureMove.To)) AttackMoves.Add(captureMove);

        return AttackMoves;
    }

    public override void ApplyMove(Move move)
    {
        base.ApplyMove(move);
        hasMoved = true;
    }

    private void TryEnPassant()
    {
        Move lastMove = GameManager.Instance.GetLastMove();
        if (lastMove == null) return;
        if (Board.Instance.GetPiece(lastMove.To) is not Pawn) return;
        if (Mathf.Abs(lastMove.Delta.y) != 2) return;
        if (Mathf.Abs(lastMove.To.x - BoardIndex.x) != 1) return;
        if(lastMove.To.y != BoardIndex.y) return;


        Move enPassantMove = new(BoardIndex, Vector2Int.zero, MoveType.EnPassant);
        enPassantMove.To = new Vector2Int(lastMove.To.x, BoardIndex.y + (Color == TeamColor.White ? 1 : -1));
        enPassantMove.CapturedSquare = lastMove.To;
        LegalMoves.Add(enPassantMove);
    }
}
