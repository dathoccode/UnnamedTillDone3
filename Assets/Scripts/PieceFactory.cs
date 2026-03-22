using System.Collections.Generic;
using UnityEngine;

public class PieceFactory : MonoBehaviour 
{
    [SerializeField] private List<PiecePrefabEntry> prefabList;

    public ChessPiece CreatePiece(PieceType type, TeamColor color, Vector2Int pos)
    {
        ChessPiece piece;
        foreach (var prefab in prefabList)
        {
            if (prefab.pieceType == type)
            {
                piece = GameObject.Instantiate(prefab.piecePrefab, 
                    new Vector3(pos.x, pos.y, 0), 
                    Quaternion.identity);
                piece.InitailizePiece(type, color);
                return piece;
            }
        }
        return null;
    }
}
