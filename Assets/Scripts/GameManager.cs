using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private Board board;
    public ChessPiece currentPiece;
    public TeamColor curTurn;
    public List<Move> currentLegalMoves = new();
    public Move LastMove { get; private set; }

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
        currentLegalMoves = currentPiece.GetLegalMoves(board);
    }

    private void ProcessMouseInput()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnMouseClicked(mouseWorld);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            OnMouseReleased(mouseWorld);
        }
    }

    private void OnMouseClicked(Vector2 mouseWorld)
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

        if (hit.collider == null) return;

        ChessPiece collidedPiece = hit.collider.gameObject.GetComponent<ChessPiece>();
        if (collidedPiece == null) return;
        if (curTurn != collidedPiece.Color) return;

        SetCurrentPiece(collidedPiece);
        currentPiece.EnableFollowMouse();
    }

    private void OnMouseReleased(Vector2 mouseWorld)
    {
        if (currentPiece == null) return;

        currentPiece.GetComponent<FollowMouse>().enabled = false;

        Vector2Int targetPos = new(Mathf.RoundToInt(mouseWorld.x), Mathf.RoundToInt(mouseWorld.y));
        Move chosenMove = currentLegalMoves.Find(move => move.MatchesTarget(currentPiece.BoardIndex, targetPos));

        if (chosenMove != null)
        {
            board.ApplyMove(chosenMove);
            LastMove = chosenMove;
            curTurn = curTurn == TeamColor.White ? TeamColor.Black : TeamColor.White;
            currentPiece = null;
            currentLegalMoves.Clear();
            return;
        }

        currentPiece.RecoverPosition();
        currentPiece = null;
        currentLegalMoves.Clear();
    }
}
