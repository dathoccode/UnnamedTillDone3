using System.Collections;
using UnityEngine;
public enum PieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}

public class PieceData
{
    public PieceType type;
    public Sprite sprie;
    public int teamColor;
}