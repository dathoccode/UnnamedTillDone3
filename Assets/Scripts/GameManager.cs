using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public ChessPiece currentPiece;
    public TeamColor curTurn;
    public List<Move> curLegalMoves = new();
    private Stack<Move> MoveStack = new();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        Board.Instance.InitPieces();
    }

    private void Update()
    {
        ProcessMouseInput();
    }

    public void SetCurrentPiece(ChessPiece newPiece)
    {
        if(currentPiece != null && currentPiece.Equals(newPiece)) return;
        currentPiece = newPiece;
        curLegalMoves = currentPiece.GetLegalMoves();
    }
    
    public Move GetLastMove()
    {
        return MoveStack.Count > 0 ? MoveStack.Peek() : null;
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

        if (hit.collider.gameObject.GetComponent<ChessPiece>() == null) return;

        ChessPiece collidedPiece = hit.collider.gameObject.GetComponent<ChessPiece>();

        if (curTurn != collidedPiece.Color) return;
        
        SetCurrentPiece(collidedPiece);
        currentPiece.EnableFollowMouse();
    }

    private void OnMouseReleased(Vector2 mouseWorld)
    {
        if (currentPiece == null) return;

        currentPiece.GetComponent<FollowMouse>().enabled = false;

        Vector2Int newPos = new (Mathf.RoundToInt(mouseWorld.x), Mathf.RoundToInt(mouseWorld.y));

        Move move = curLegalMoves.Find(m => m.To == newPos && m.From == currentPiece.BoardIndex);

        if (move != null)
        {
            MovePiece(move);
        }
        else
        {
            currentPiece.RecoverPosition();
        }
    }

    public void MovePiece(Move move)
    {
        
        if (!Board.Instance.ApplyMove(move))
        {
            currentPiece.RecoverPosition();
            return;
        }

        currentPiece.ApplyMove(move);
        MoveStack.Push(move);
        curTurn = curTurn == TeamColor.White ? TeamColor.Black : TeamColor.White;
        currentPiece = null;
    }
}
