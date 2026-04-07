using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private Board board;
    public ChessPiece currentPiece;
    public TeamColor curTurn;
    public List<Vector2Int> curValidMoves = new List<Vector2Int>();

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

        List<Vector2Int> array = currentPiece.GetAllValidMoves(board);
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

        // check if clicked object is a chess piece
        if (hit.collider != null && hit.collider.gameObject.GetComponent<ChessPiece>() != null)
        {
            
            ChessPiece collidedPiece = hit.collider.gameObject.GetComponent<ChessPiece>();

            if (curTurn == collidedPiece.Color)
            {
                // Set collided piece to be the current piece
                SetCurrentPiece(collidedPiece);

                // Make piece follow mouse position
                currentPiece.EnableFollowMouse();
                // Take all valid moves of the piece
                curValidMoves = collidedPiece.GetAllValidMoves(board);
            }
        }
    }

    private void OnMouseReleased(Vector2 mouseWorld)
    {
        if (currentPiece == null) return;

        // Disable follow mouse position component of chess piece
        currentPiece.GetComponent<FollowMouse>().enabled = false;

        Vector2Int newPos = new (Mathf.RoundToInt(mouseWorld.x), Mathf.RoundToInt(mouseWorld.y));

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
        
        if (!board.OnPieceMove(currentPiece.BoardIndex, newPos))
        {
            return;
        }

        // Move piece in world
        currentPiece.MoveTo(newPos);

        // Switch turn
        curTurn = curTurn == TeamColor.White ? TeamColor.Black : TeamColor.White;

        // Reset current piece
        currentPiece = null;
    }

    private void HandleCastling()
    {
        if (currentPiece.PieceSO.type != PieceType.King) return;
    }
}
