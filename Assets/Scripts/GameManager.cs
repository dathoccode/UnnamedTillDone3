using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MyMonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private Board board;
    public ChessPiece currentPiece;  

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (board == null) Debug.LogError("Board is not set in " + gameObject.name);
    }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    protected override void Start()
    {
        base.Start();
        board.InitPieces();
    }

    public void SetCurrentPiece(ChessPiece newPiece)
    {
        currentPiece = newPiece;
    }

    public void OnPieceMove(Vector2Int newPos)
    {
        if (!board.Move(currentPiece.BoardIndex, newPos)) 
        {
            currentPiece.RecoverPosition();
        }
        else currentPiece.MoveTo(newPos);

    }
}
