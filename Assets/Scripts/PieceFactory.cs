using UnityEngine;

public class PieceFactory
{
    public static ChessPiece CreatePiece(PieceType type, TeamColor color)
    {
        switch (type)
        {
            case PieceType.Pawn: return new Pawn(color);
            case PieceType.Rook: return new Rook(color);
            case PieceType.Knight: return new Knight(color);
            case PieceType.Bishop: return new Bishop(color);
            case PieceType.Queen: return new Queen(color);
            case PieceType.King: return new King(color);
            default: return null;
        }
    }
}
