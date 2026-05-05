using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    // Starting positions for white pieces (black pieces would be mirrored on the opposite side (x = whitepiece.x, y = 7 - whitepiece.y))
    public static readonly Dictionary<PieceType, Vector2Int[]> WhiteStartPositions = new Dictionary<PieceType, Vector2Int[]> 
    {
        {
            PieceType.Pawn, new Vector2Int[]
            {
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(2, 1),
                new Vector2Int(3, 1),
                new Vector2Int(4, 1),
                new Vector2Int(5, 1),
                new Vector2Int(6, 1),
                new Vector2Int(7, 1)
            }
        },
        {
            PieceType.Knight, new Vector2Int[]
            {
                new Vector2Int(1, 0),
                new Vector2Int(6, 0)
            }
        },
        {
            PieceType.King, new Vector2Int[]
            {
                new Vector2Int(4, 0)
            }
        },
        {
            PieceType.Bishop, new Vector2Int[]
            {
                new Vector2Int(2, 0),
                new Vector2Int(5, 0)
            }
        },
        {
            PieceType.Queen, new Vector2Int[]
            {
                new Vector2Int(3, 0)
            }
        },
        {
            PieceType.Rook, new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(7, 0)
            }
        },
    };

}
