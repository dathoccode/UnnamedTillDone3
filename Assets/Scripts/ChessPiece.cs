using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChessPiece : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public PieceData PieceSO;
    public TeamColor Color;
    public Vector2Int BoardIndex;

    public ChessPiece InstantiatePiece(PieceType type, TeamColor color)
    {
        
        this.PieceSO = Resources.Load<PieceData>("PieceData/" + type.ToString());
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = color == TeamColor.White ? PieceSO.whiteSprite : PieceSO.blackSprite;

        BoardIndex = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.y);
        return this;
    }

    public void MoveTo(Vector2Int newPos)
    {
        BoardIndex = newPos;
        transform.position = new Vector2(newPos.x, newPos.y);
    }

    public void RecoverPosition()
    {
        transform.position = new Vector2(BoardIndex.x, BoardIndex.y);
        Debug.Log("recovered to " + transform.position);
    }
}
