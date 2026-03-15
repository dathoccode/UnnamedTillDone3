using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChessPiece : MonoBehaviour
{
    private PieceData data;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private TeamColor color;
    private Vector2Int position { get; set; }

    private void Start()
    {
        Debug.Log("ChessPiece Start: " + gameObject.name);
    }

    void Update()
    {
        
    }

    public ChessPiece InstantiatePiece(PieceType type, TeamColor color)
    {
        this.data = Resources.Load<PieceData>("PieceData/" + type.ToString());
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = color == TeamColor.White ? data.whiteSprite : data.blackSprite;
        return this;
    }
}
