using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ChessPiece[,] pieces = new ChessPiece[8,8];
    public ChessPiece piecePrefab;
    void Start()
    {
        InitPieces();
    }

    void Update()
    {
        
    }

    void InitPieces()
    {
        foreach(var piece in Data.WhiteStartPositions)
        {
            foreach(var position in piece.Value)
            {
                ChessPiece newWhitePiece = Instantiate(piecePrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
                newWhitePiece.InstantiatePiece(piece.Key, TeamColor.White);
                pieces[position.x, position.y] = newWhitePiece;
                ChessPiece newBlackPiece = Instantiate(piecePrefab, new Vector3(position.x, 7 -position.y, 0), Quaternion.identity);
                newBlackPiece.InstantiatePiece(piece.Key, TeamColor.Black);
                pieces[position.x, 7 - position.y] = newBlackPiece;
            }
        }
    }
}
