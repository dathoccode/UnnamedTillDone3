using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public bool hasMoved;

    public override List<Move> GetLegalMoves()
    {
        base.GetLegalMoves();
        TryCastleMove();

        return LegalMoves;
    }

    private void TryCastleMove()
    {
        if (hasMoved) return;
        if (LegalMoves.Count == 0) return;

        if (TryCastleKingSide()) LegalMoves.Add(new Move(BoardIndex, BoardIndex + new Vector2Int(2, 0), MoveType.Castling));
        if (TryCastleQueenSide()) LegalMoves.Add(new Move(BoardIndex, BoardIndex + new Vector2Int(-2, 0), MoveType.Castling));
    }

    private bool TryCastleKingSide()
    {
        Rook rook = Board.Instance.GetPiece(7, BoardIndex.y) as Rook;

        if (rook == null) return false;
        if (rook.HasMove) return false;


        // ô giữa phải trống
        if (Board.Instance.GetPiece(5, BoardIndex.y) != null) return false;
        if (Board.Instance.GetPiece(6, BoardIndex.y) != null) return false;

        // không bị attack
        if (Board.Instance.IsSquareAttacked(new Vector2Int(4, BoardIndex.y), Color)) return false;
        if (Board.Instance.IsSquareAttacked(new Vector2Int(5, BoardIndex.y), Color)) return false;
        if (Board.Instance.IsSquareAttacked(new Vector2Int(6, BoardIndex.y), Color)) return false;

        return true;
    }

    private bool TryCastleQueenSide()
    {
        Rook rook = Board.Instance.GetPiece(0, BoardIndex.y) as Rook;

        if (rook == null) return false;
        if (rook.HasMove) return false;

        // ô giữa phải trống
        if (Board.Instance.GetPiece(1, BoardIndex.y) != null) return false;
        if (Board.Instance.GetPiece(2, BoardIndex.y) != null) return false;
        if (Board.Instance.GetPiece(3, BoardIndex.y) != null) return false;

        // không bị attack
        if (Board.Instance.IsSquareAttacked(new Vector2Int(4, BoardIndex.y), Color)) return false;
        if (Board.Instance.IsSquareAttacked(new Vector2Int(3, BoardIndex.y), Color)) return false;
        if (Board.Instance.IsSquareAttacked(new Vector2Int(2, BoardIndex.y), Color)) return false;

        return true;
    }


    public override void ApplyMove(Move move)
    {
        if (move.Type == MoveType.Castling)
        {
            if (move.To.x == 6)
            {
                Rook rook = Board.Instance.GetPiece(7, BoardIndex.y) as Rook;
                rook.ApplyMove(new Move(rook.BoardIndex, new Vector2Int(5, BoardIndex.y), MoveType.Castling));
            }
            else if (move.To.x == 2)
            {
                Rook rook = Board.Instance.GetPiece(0, BoardIndex.y) as Rook;
                rook.ApplyMove(new Move(rook.BoardIndex, new Vector2Int(3, BoardIndex.y), MoveType.Castling));
            }

        }
        hasMoved = true;
        base.ApplyMove(move);
    }
}
