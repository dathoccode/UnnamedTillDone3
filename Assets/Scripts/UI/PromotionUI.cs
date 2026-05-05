using UnityEngine;
using UnityEngine.UI;

public class PromotionUI : BaseUIController
{
    [SerializeField]
    private Button queenButton;
    [SerializeField]
    private Button knightButton;
    [SerializeField]
    private Button bishopButton;
    [SerializeField]
    private Button rookButton;

    private void Start()
    {
        if(queenButton == null || knightButton == null || bishopButton == null || rookButton == null)
        {
            Debug.LogError("PromotionUI: One or more buttons are not assigned in the inspector.");
            return;
        }
        queenButton.onClick.AddListener(OnQueenSelected);
        knightButton.onClick.AddListener(OnKnightSelected);
        bishopButton.onClick.AddListener(OnBishopSelected);
        rookButton.onClick.AddListener(OnRookSelected);
    }

    private void OnQueenSelected()
    {
        GameManager.Instance.PromotePawn(PieceType.Queen);
    }

    private void OnKnightSelected()
    {
        GameManager.Instance.PromotePawn(PieceType.Knight);
    }

    private void OnRookSelected()
    {
        GameManager.Instance.PromotePawn(PieceType.Rook);
    }

    private void OnBishopSelected()
    {
        GameManager.Instance.PromotePawn(PieceType.Bishop);
    }
}
