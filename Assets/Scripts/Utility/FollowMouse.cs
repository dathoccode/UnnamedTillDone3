using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        Vector3 cam = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        this.transform.position = new Vector3(cam.x, cam.y);
    
    }
}
