using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public bool hasMoved;

    public override  List<Vector2Int> GetAllValidMoves(Board board)
    {
        List<Vector2Int> validMoves = base.GetAllValidMoves(board);

        if (!hasMoved)
        {
            ChessPiece leftRook = board.GetPiece(0, BoardIndex.y);
            if (leftRook != null && leftRook.PieceSO.type == PieceType.Rook) {
                if (!leftRook.GetComponent<Rook>().hasMoved)
                {
                    validMoves.Add(new Vector2Int(-2, 0));
                }
            }

            ChessPiece rightRook = board.GetPiece(7, BoardIndex.y);
            if (rightRook != null && rightRook.PieceSO.type == PieceType.Rook)
            {
                if (!rightRook.GetComponent<Rook>().hasMoved)
                {
                    validMoves.Add(new Vector2Int(2, 0));
                }
            }
        }
       
        return validMoves;
    }

    public override void MoveTo(Vector2Int newPos)
    {
        base.MoveTo(newPos);
        hasMoved = true;
    }
}
