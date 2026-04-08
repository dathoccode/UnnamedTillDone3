using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public bool hasMoved;

    public override List<Vector2Int> GetSpecialMoves(Board board)
    {
        List<Vector2Int> specialMoves = base.GetSpecialMoves(board);

        specialMoves.AddRange(AddCastleMoves(board));

        return specialMoves;
    }

    private List<Vector2Int> AddCastleMoves(Board board)
    {
        
        if (hasMoved) return null;
        List<Vector2Int> moves = new();

        if (TryCastleKingSide(board)) moves.Add(new Vector2Int(2, 0));
        if (TryCastleQueenSide(board)) moves.Add(new Vector2Int(-2, 0));
        return moves;
    }

    private bool TryCastleKingSide(Board board)
    {
        Rook rook = board.GetPiece(7, BoardIndex.y) as Rook;

        if (rook == null)
        {
            Debug.Log("No rook for king side castling");
            return false;
        }
        if (rook.HasMove)
        {
            Debug.Log("Rook has moved for king side castling");
            return false;
        }

        // ô giữa phải trống
        if (board.GetPiece(5, BoardIndex.y) != null)
        {
            Debug.Log("Square 5 is not empty for king side castling");
            return false;
        }
        if (board.GetPiece(6, BoardIndex.y) != null)
        {
            Debug.Log("Square 6 is not empty for king side castling");
            return false;
        }

        // không bị attack
        if (board.IsSquareAttacked(new Vector2Int(4, BoardIndex.y), Color))
        {
            Debug.Log("Square 4 is attacked for king side castling");
            return false;
        }
        if (board.IsSquareAttacked(new Vector2Int(5, BoardIndex.y), Color))
        {             
            Debug.Log("Square 5 is attacked for king side castling");
            return false;
        }
        if (board.IsSquareAttacked(new Vector2Int(6, BoardIndex.y), Color))
        {
            Debug.Log("Square 6 is attacked for king side castling");
            return false;
        }

        Debug.Log("Can castle king side");
        return true;
    }

    private bool TryCastleQueenSide(Board board)
    {
        Rook rook = board.GetPiece(0, BoardIndex.y) as Rook;

        if (rook == null) return false;
        if (rook.HasMove) return false;

        // ô giữa phải trống
        if (board.GetPiece(1, BoardIndex.y) != null) return false;
        if (board.GetPiece(2, BoardIndex.y) != null) return false;
        if (board.GetPiece(3, BoardIndex.y) != null) return false;

        // không bị attack
        if (board.IsSquareAttacked(new Vector2Int(4, BoardIndex.y), Color)) return false;
        if (board.IsSquareAttacked(new Vector2Int(3, BoardIndex.y), Color)) return false;
        if (board.IsSquareAttacked(new Vector2Int(2, BoardIndex.y), Color)) return false;

        return true;
    }


    public override void MoveTo(Vector2Int newPos)
    {
        Debug.Log($"King move from {BoardIndex} to {newPos}");
        if (Mathf.Abs(newPos.x - BoardIndex.x) == 2)
        {
            // castling move
            if (newPos.x == 6)
            {
                // kingside
                Rook rook = Board.Instance.GetPiece(7, BoardIndex.y) as Rook;
                if (rook == null)
                {
                    Debug.LogError("No rook found for castling move");
                    return;
                }
                rook.HandleCastlingRookMove();
            }
            else if (newPos.x == 2)
            {
                // queenside
                Rook rook = Board.Instance.GetPiece(0, BoardIndex.y) as Rook;
                if (rook == null)
                {
                    Debug.LogError("No rook found for castling move");
                    return;
                }
                rook.HandleCastlingRookMove();
            }
            
        }
        hasMoved = true;
        base.MoveTo(newPos);
    }
}
