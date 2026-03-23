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
                SpawnPiece(piece.Key, position, TeamColor.White);
                SpawnPiece(piece.Key, new Vector2Int(position.x, 7 - position.y), TeamColor.Black);
            }
        }
    }

    private void SpawnPiece(PieceType type, Vector2Int pos, TeamColor color)
    {
        GameObject obj = Instantiate(piecePrefab,
            new Vector3(pos.x, pos.y, 0),
            Quaternion.identity);

        ChessPiece piece = AddPieceComponent(type, obj);
        piece.InitailizePiece(type, color);

        piece.transform.SetParent(holder.transform, false);
        piece.name = $"{color}_{type}_{pos.x}_{pos.y}";

        grid[pos.x, pos.y] = piece;
    }

    public void MoveOnBoard(Vector2Int from, Vector2Int to)
    {
        if (grid[to.x, to.y] != null)
        {
            Destroy(grid[to.x, to.y].gameObject);
        }
        grid[to.x, to.y] = grid[from.x, from.y];
        grid[from.x, from.y] = null;
    }

    public ChessPiece GetPiece(Vector2Int pos)
    {
        return grid[pos.x, pos.y];
    }

    public ChessPiece GetPiece(int x, int y)
    {
        return grid[x, y];
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
