using UnityEngine;

public struct MoveModel
{
    public ChessPiece Piece;
    public Vector2Int From;
    public Vector2Int To;
    public ChessPiece CapturedPiece;
    public Vector2Int CapturedPosition;
    public bool IsEnPassant;
    public bool IsPromotion;

    public Vector2Int Delta => To - From;
}
