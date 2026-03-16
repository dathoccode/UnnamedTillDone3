using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        // Click chuột
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                GameManager.Instance.SetCurrentPiece(GetComponent<ChessPiece>());
            }
        }

        // Thả chuột
        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            GameManager.Instance.OnPieceMove(FitInBoard());
            
        }

        // Kéo object
        if (isDragging)
        {
            transform.position = new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z);
        }
    }

    Vector2Int FitInBoard()
    {
       return new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y));
    }
}