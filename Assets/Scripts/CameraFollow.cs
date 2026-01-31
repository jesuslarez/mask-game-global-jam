using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player to follow
    public Vector3 offset;   // Offset to maintain distance from the player

    private void LateUpdate()
    {
        // Update the camera's position to follow the player
        if (target != null)
        {
            transform.position = target.position + offset;
        }   
    }
}