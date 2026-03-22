using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public ChessPiece[,] grid = new ChessPiece[8, 8];
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject holder;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void InitPieces()
    {
        foreach (var piece in Data.WhiteStartPositions)
        {
            foreach (var position in piece.Value)
            {
                GameObject prefab = Instantiate(piecePrefab,
                    new Vector3(position.x, position.y, 0),
                    Quaternion.identity);
                ChessPiece newWhitePiece = AddPieceComponent(piece.Key, prefab);
                newWhitePiece.InitailizePiece(piece.Key, TeamColor.White);
                newWhitePiece.transform.SetParent(holder.transform, false);
                newWhitePiece.name = piece.Key.ToString() + position;
                grid[position.x, position.y] = newWhitePiece;


                GameObject prefab2 = Instantiate(piecePrefab,
                    new Vector3(position.x, 7 - position.y, 0),
                    Quaternion.identity);
                ChessPiece newBlackPiece = AddPieceComponent(piece.Key, prefab2);
                newBlackPiece.InitailizePiece(piece.Key, TeamColor.Black);
                newBlackPiece.transform.SetParent(holder.transform, false);
                newBlackPiece.name = piece.Key.ToString() + position;
                grid[position.x, 7 - position.y] = newBlackPiece;
            }
        }
    }

    public void MoveOnBoard(Vector2Int from, Vector2Int to)
    {
        grid[to.x, to.y] = grid[from.x, from.y];
        grid[from.x, from.y] = null;

    }

    private ChessPiece AddPieceComponent(PieceType type, GameObject prefab)
    {
        switch (type)
        {
            case PieceType.Pawn:
                return prefab.AddComponent<Pawn>();
            case PieceType.Rook:
                return prefab.AddComponent<Rook>();
            case PieceType.King:
                return prefab.AddComponent<King>();
            default:
                return prefab.AddComponent<Queen>();
        }
    }
}
