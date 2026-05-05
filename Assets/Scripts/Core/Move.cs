using System;
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

    public Move()
    {

    }

    public Move(Vector2Int from, Vector2Int to, MoveType type = MoveType.Normal)
    {
        From = from;
        To = to;
        Type = type;
        CapturedSquare = to;
    }

    public Vector2Int Delta => To - From;

    public bool MatchesTarget(Vector2Int origin, Vector2Int target)
    {
        return From == origin && To == target;
    }

    public override bool Equals(object obj)
    {
        if (obj is Move other)
        {
            return this.From == other.From && this.To == other.To;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(From, To);
    }
}
