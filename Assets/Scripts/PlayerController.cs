using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement
    public Sprite spriteUp;     // Sprite for looking up
    public Sprite spriteDown;   // Sprite for looking down
    public Sprite spriteSide;   // Sprite for looking left/right

    private Vector3 moveDirection;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Reset movement direction 
        moveDirection = Vector3.zero;

        // Get input for basic left, right, up, and down movement
        if (Keyboard.current.wKey.isPressed) moveDirection.y = 1f;  // Move up
        if (Keyboard.current.sKey.isPressed) moveDirection.y = -1f; // Move down
        if (Keyboard.current.aKey.isPressed) moveDirection.x = -1f; // Move left
        if (Keyboard.current.dKey.isPressed) moveDirection.x = 1f;  // Move right

        // Normalize the direction to ensure consistent speed
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // Move the player smoothly
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Update the sprite based on movement direction
        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        if (moveDirection.x > 0) // Moving right
        {
            spriteRenderer.sprite = spriteSide;
            spriteRenderer.flipX = false; // Face right
        }
        else if (moveDirection.x < 0) // Moving left
        {
            spriteRenderer.sprite = spriteSide;
            spriteRenderer.flipX = true; // Face left
        }
        else if (moveDirection.y > 0) // Moving up
        {
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false; // No flip needed
        }
        else if (moveDirection.y < 0) // Moving down
        {
            spriteRenderer.sprite = spriteDown;
            spriteRenderer.flipX = false; // No flip needed
        }
    }
}