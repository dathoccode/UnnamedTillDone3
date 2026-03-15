using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector2 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);

        // Click chuột
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
            }
        }

        // Thả chuột
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
            FitInBoard();
        }

        // Kéo object
        if (isDragging)
        {
            transform.position = new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z);
        }
    }

    void FitInBoard()
    {
        transform.position = new Vector2(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y));
    }
}