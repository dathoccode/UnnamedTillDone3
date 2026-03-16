using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MyMonoBehaviour
{
    public ChessPiece[,] pieces = new ChessPiece[8, 8];
    [SerializeField] private ChessPiece piecePrefab;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject holder;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        tilemap = GetComponent<Tilemap>();
        if (tilemap == null) Debug.LogError("Tile map is not set in " + gameObject.name);
        if (piecePrefab == null) Debug.LogError("Piece Prefab is not set in " + gameObject.name);
        if (holder == null) Debug.LogError("Holder is not set in " + gameObject.name);
    
    }

    public void InitPieces()
    {
        int i = 0;
        foreach (var piece in Data.WhiteStartPositions)
        {
            foreach (var position in piece.Value)
            {
                ChessPiece newWhitePiece = Instantiate(piecePrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
                newWhitePiece.InstantiatePiece(piece.Key, TeamColor.White);
                pieces[position.x, position.y] = newWhitePiece;
                ChessPiece newBlackPiece = Instantiate(piecePrefab, new Vector3(position.x, 7 - position.y, 0), Quaternion.identity);
                newBlackPiece.InstantiatePiece(piece.Key, TeamColor.Black);
                pieces[position.x, 7 - position.y] = newBlackPiece;
                newWhitePiece.transform.parent = holder.transform;
                newBlackPiece.transform.parent = holder.transform;

                newWhitePiece.name = i++.ToString();
                newBlackPiece.name = i++.ToString();
            }
        }
    }

    public bool Move(Vector2Int from, Vector2Int to)
    {
        if (!ValidateMove(from, to))
        {
            Debug.Log("Move failed");
            return false;
        }
        Destroy(pieces[to.x, to.y]);
        pieces[to.x, to.y] = pieces[from.x, from.y];
        pieces[from.x, from.y] = null;
        Debug.Log("Move successfully");
        return true;
    }

    private void Take(Vector2Int pos)
    {
        //TODO: Take implementation
        pieces[pos.x, pos.y] = null;
    }

    private bool ValidateMove(Vector2Int from, Vector2Int to)
    {
        Vector2Int dir = NormalizeDir(to - from);
        ChessPiece fromPiece = pieces[from.x, from.y];
        Vector2Int pos = from + dir;
        // piece doesn't exist / destination doesn't fit move pattern
        if (fromPiece == null || !fromPiece.PieceSO.movePatterns.Contains(dir)) 
            return false;

        while (pos != to)
        {
            if (pos == to)
            {
                ChessPiece targetPiece = pieces[to.x, to.y];
                // destination cell already had an ally piece
                if (targetPiece == null || targetPiece.Color != fromPiece.Color) 
                    return true;
            }

            // There is another piece on the way
            if (pieces[pos.x, pos.y] != null) return false;

            pos += dir;
        }

        return false;
    }

    private Vector2Int NormalizeDir(Vector2Int dir)
    {
        int gcd = GCD(dir.x, dir.y);
        int x = dir.x / Mathf.Abs(gcd);
        int y = dir.y / Mathf.Abs(gcd);

        return new Vector2Int(x, y);
    }

    int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
}
