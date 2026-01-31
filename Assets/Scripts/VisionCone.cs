using UnityEngine;
using UnityEngine.InputSystem;

public class VisionCone : MonoBehaviour
{
    void Update()
    {
        // Posición del mouse en mundo
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        Vector3 direction = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.up = direction;
    }
}