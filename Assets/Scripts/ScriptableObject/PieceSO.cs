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

public enum TeamColor
{
    White,
    Black
}

[CreateAssetMenu(fileName = "PieceData", menuName = "ScriptableObjects/PieceData")]
public class PieceSO : ScriptableObject
{
    public PieceType type;
    public Sprite whiteSprite;
    public Sprite blackSprite;
    public Vector2Int[] movePatterns;
    public bool isSliding;
}