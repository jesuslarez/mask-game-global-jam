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
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    private PowerUp currentPowerUp; // The currently collected power-up
    private bool isUsingPowerUp = false; // Flag to prevent multiple activations
    private bool isInvulnerable = false; // Flag for invulnerability

    private void Start()
    {
        // Get the SpriteRenderer and Rigidbody2D components
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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

        // Update the sprite based on movement direction
        UpdateSpriteDirection();

        // Activate the power-up when "E" is pressed
        if (Keyboard.current.eKey.wasPressedThisFrame && currentPowerUp != null && !isUsingPowerUp)
        {
            UsePowerUp();
        }
    }

    private void FixedUpdate()
    {
        // Move the player using Rigidbody2D
        rb.linearVelocity = moveDirection * moveSpeed;
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

    public void CollectPowerUp(PowerUp powerUp)
    {
        currentPowerUp = powerUp; // Store the collected power-up
        Debug.Log($"Collected Power-Up: {powerUp.powerUpType}");
    }

    private void UsePowerUp()
    {
        if (currentPowerUp == null) return;

        Debug.Log($"Using Power-Up: {currentPowerUp.powerUpType}");
        isUsingPowerUp = true;

        switch (currentPowerUp.powerUpType)
        {
            case PowerUp.PowerUpType.SpeedBoost:
                StartCoroutine(SpeedBoost());
                break;

            case PowerUp.PowerUpType.Invulnerability:
                StartCoroutine(Invulnerability());
                break;

            // Add more power-up types here
        }

        currentPowerUp = null; // Clear the power-up after use
    }

    private System.Collections.IEnumerator SpeedBoost()
    {
        float originalSpeed = moveSpeed;
        moveSpeed *= 3; // SUPER speed (times 3)
        yield return new WaitForSeconds(5f); // Duration of the speed boost
        moveSpeed = originalSpeed; // Reset to original speed
        isUsingPowerUp = false;
    }

    private System.Collections.IEnumerator Invulnerability()
    {
        isInvulnerable = true; // Activate invulnerability
        yield return new WaitForSeconds(5f); // Duration of invulnerability
        isInvulnerable = false; // Deactivate invulnerability
        isUsingPowerUp = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (isInvulnerable)
            {
                Debug.Log("Player is invulnerable. No damage taken.");
                return;
            }

            Debug.Log("Player collided with an enemy. Game Over!");
            Die();
        }
    }

    private void Die()
    {
        // Handle player death (e.g., end the game)
        Debug.Log("Player has died. Ending the game...");
        Time.timeScale = 0; // Pause the game
    }
}