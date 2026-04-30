using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private Board board;
    public ChessPiece currentPiece;
    public TeamColor curTurn = TeamColor.White;
    public List<Vector2Int> curValidMoves = new();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        board.InitPieces();
    }

    private void Update()
    {
        ProcessMouseInput();
    }

    public void SetCurrentPiece(ChessPiece newPiece)
    {
        currentPiece = newPiece;
        curValidMoves = currentPiece.GetValidMoves(board);
    }

    private void ProcessMouseInput()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        if (Mouse.current.leftButton.wasPressedThisFrame) OnMouseClicked(mouseWorld);
        if (Mouse.current.leftButton.wasReleasedThisFrame) OnMouseReleased(mouseWorld);
    }

    private void OnMouseClicked(Vector2 mouseWorld)
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);
        if (hit.collider == null) return;

        ChessPiece collidedPiece = hit.collider.gameObject.GetComponent<ChessPiece>();
        if (collidedPiece == null || curTurn != collidedPiece.Color) return;

        SetCurrentPiece(collidedPiece);
        currentPiece.EnableFollowMouse();
    }

    private void OnMouseReleased(Vector2 mouseWorld)
    {
        if (currentPiece == null) return;

        currentPiece.GetComponent<FollowMouse>().enabled = false;

        Vector2Int newPos = new(Mathf.RoundToInt(mouseWorld.x), Mathf.RoundToInt(mouseWorld.y));
        Vector2Int move = newPos - currentPiece.BoardIndex;

        if (curValidMoves.Contains(move))
        {
            MovePiece(newPos);
        }
        else
        {
            currentPiece.RecoverPosition();
        }
    }

    public void MovePiece(Vector2Int newPos)
    {
        if (!board.TryMovePiece(currentPiece, newPos, out MoveModel move))
        {
            currentPiece.RecoverPosition();
            return;
        }

        currentPiece.MoveTo(newPos);
        HandlePromotion(move);

        curTurn = curTurn == TeamColor.White ? TeamColor.Black : TeamColor.White;
        currentPiece = null;
        curValidMoves.Clear();
    }

    private void HandlePromotion(MoveModel move)
    {
        if (!move.IsPromotion || move.Piece is not Pawn pawn) return;
        board.PromotePawn(pawn, PieceType.Queen);
    }
}
