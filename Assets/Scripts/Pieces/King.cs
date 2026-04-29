using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public bool hasMoved;

    public override List<Move> GetLegalMoves(Board board)
    {
        List<Move> legalMoves = base.GetLegalMoves(board);

        if (!hasMoved)
        {
            ChessPiece leftRook = board.GetPiece(0, BoardIndex.y);
            if (leftRook != null && leftRook.PieceSO.type == PieceType.Rook)
            {
                if (!leftRook.GetComponent<Rook>().hasMoved)
                {
                    legalMoves.Add(new Move(BoardIndex, BoardIndex + new Vector2Int(-2, 0), MoveType.Castling));
                }
            }

            ChessPiece rightRook = board.GetPiece(7, BoardIndex.y);
            if (rightRook != null && rightRook.PieceSO.type == PieceType.Rook)
            {
                if (!rightRook.GetComponent<Rook>().hasMoved)
                {
                    legalMoves.Add(new Move(BoardIndex, BoardIndex + new Vector2Int(2, 0), MoveType.Castling));
                }
            }
        }

        return legalMoves;
    }
}
