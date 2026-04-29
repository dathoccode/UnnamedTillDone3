using UnityEngine;

public enum MoveType
{
    Normal,
    Capture,
    EnPassant,
    Castling,
    Promotion
}

public class Move
{
    public Vector2Int From;
    public Vector2Int To;
    public MoveType Type;
    public Vector2Int CapturedSquare;

    public Move(Vector2Int from, Vector2Int to, MoveType type = MoveType.Normal)
    {
        From = from;
        To = to;
        Type = type;
        CapturedSquare = to;
    }

    public Vector2Int Delta => To - From;

    public bool MatchesDelta(Vector2Int origin, Vector2Int delta)
    {
        return From == origin && Delta == delta;
    }
}
