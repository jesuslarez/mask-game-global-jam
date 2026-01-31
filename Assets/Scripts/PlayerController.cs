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

    public bool hasKey; // The currently collected key
    private bool isUsingPowerUp = false; // Flag to prevent multiple activations
    private bool isInvulnerable = false; // Flag for invulnerability
    public PowerUpType? currentPowerUpType; // nullable
    private SpawnController spawnController;

    private UI_MaskEquipped uiMask;
    private UI_Key uiKey;


    private void Start()
    {
        // Get the SpriteRenderer and Rigidbody2D components
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spawnController = FindAnyObjectByType<SpawnController>(); 
        uiMask = FindAnyObjectByType<UI_MaskEquipped>();
        uiKey = FindAnyObjectByType<UI_Key>();

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
        if (Keyboard.current.eKey.wasPressedThisFrame && currentPowerUpType != null && !isUsingPowerUp)
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

    public void CollectPowerUp(PowerUpType newType)
    {
        if (currentPowerUpType != null)
        {
            spawnController.DropMaskAt(transform.position, currentPowerUpType.Value);
        }

        currentPowerUpType = newType;
        Debug.Log(newType);
    }


    public void CollectKey()
    {
        if (hasKey)
        {
            Debug.Log("Player already has a key. Cannot collect another.");
            return; // Prevent collecting another key
        }
        uiKey.KeyGained();
        hasKey = true; 
       
    }


    public void UseKey()
    {
        uiKey.KeyUsed();

    }

    private void UsePowerUp()
    {
        if (currentPowerUpType == null) return;

        isUsingPowerUp = true;

        switch (currentPowerUpType.Value)
        {
            case PowerUpType.SpeedBoost:
                StartCoroutine(SpeedBoost());
                break;

            case PowerUpType.Invulnerability:
                StartCoroutine(Invulnerability());
                break;

            // Add more power-up types here
        }
        spawnController.onPowerUpUsed();
        uiMask?.PlayUseShake();

        currentPowerUpType = null;
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
        // Handle collision with enemies
    }

    public void OnEnemyCollision()
    {
        if (isInvulnerable)
        {
            Debug.Log("Player is invulnerable and ignored the enemy collision.");
            return; // Ignore damage if invulnerable
        }
        Die();
    }

    public void Die()
    {
        // Handle player death (e.g., end the game)
        Debug.Log("Player has died. Ending the game...");
        Time.timeScale = 0; // Pause the game
    }
}
