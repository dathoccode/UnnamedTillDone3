using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static readonly PieceData[] pieceList;

    public static readonly Dictionary<PieceType, Vector2Int[]> movePatterns = new Dictionary<PieceType, Vector2Int[]>
    {
        {
            PieceType.Pawn, new Vector2Int[]
            {
                new Vector2Int(0, 1), // Move forward
                new Vector2Int(0, 2), // Initial double move
                new Vector2Int(-1, 1), // Capture left
                new Vector2Int(1, 1) // Capture right
            }
        },
        {
            PieceType.Rook, new Vector2Int[]
            {
                new Vector2Int(0, 1), // Move up
                new Vector2Int(0, -1), // Move down
                new Vector2Int(-1, 0), // Move left
                new Vector2Int(1, 0) // Move right
            }
        },
        {
            PieceType.Knight, new Vector2Int[]
            {
                new Vector2Int(1, 2), // L-shape moves
                new Vector2Int(1, -2),
                new Vector2Int(-1, 2),
                new Vector2Int(-1, -2),
                new Vector2Int(2, 1),
                new Vector2Int(2, -1),
                new Vector2Int(-2, 1),
                new Vector2Int(-2, -1)
            }
        },
        {
            PieceType.Bishop, new Vector2Int[]
            {
                new Vector2Int(1, 1), // Diagonal moves
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1)
            }
        },
        {
            PieceType.Queen, new Vector2Int[]
            {
                new Vector2Int(0, 1), // Rook-like moves
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1), // Bishop-like moves
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1)
            }
        },
        {
            PieceType.King, new Vector2Int[]
            {
                new Vector2Int(0, 1), // One square in any direction
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),
            }
        }
    };

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
