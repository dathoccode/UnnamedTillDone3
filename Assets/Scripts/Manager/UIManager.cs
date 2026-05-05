using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    [SerializeField]
    private PromotionUI promotionUI;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    
    public void ShowPromotionUI(TeamColor color)
    {
        promotionUI.Show();
        promotionUI.GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(-90, color == TeamColor.White ? 50 : 760);
    }

    public void HidePromotionUI()
    {
        promotionUI.Hide();
    }
}
