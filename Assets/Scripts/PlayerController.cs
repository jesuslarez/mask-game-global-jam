using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;

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

    [Header("Field Of View")]
    [SerializeField] private float fov;
    [SerializeField] private float distanceOfView;

    private void Start()
    {
        // Get the SpriteRenderer and Rigidbody2D components
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spawnController = FindAnyObjectByType<SpawnController>();
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

        Vector3 targetPosition = GetMouseWorldPosition();
        Vector3 aimDir = (targetPosition - transform.position).normalized;
        fieldOfView.SetAimDirection(aimDir);
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(distanceOfView);
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

        hasKey = true; 
       
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

            case PowerUpType.Stun:
                StunEnemies();
                break;

            // Add more power-up types here
        }

        spawnController.onPowerUpUsed();
        currentPowerUpType = null;
    }

    private void StunEnemies()
    {
        Debug.Log("Stun Power-Up activated!");

        // Find all enemies in the scene
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        foreach (EnemyAI enemy in enemies)
        {
            // Check if the enemy is within a certain range of the player
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= 5f) // Example range of 5 units
            {
                enemy.Stun(5f); // Stun the enemy for 5 seconds
            }
        }

        isUsingPowerUp = false; // Reset the power-up usage flag
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
        if (collision.CompareTag("Enemy")) {
            if (isInvulnerable)
            {
                Debug.Log("Player is invulnerable and ignored the enemy collision.");
                return; // Ignore damage if invulnerable
            }else {
                Die();
            }
        }

    }

    public void Die()
    {
        // Handle player death (e.g., end the game)
        Debug.Log("Player has died. Ending the game...");
        Time.timeScale = 0; // Pause the game
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        if (worldCamera == null) worldCamera = Camera.main;
        return worldCamera.ScreenToWorldPoint(screenPosition);
    }

    // Obtiene la posici�n del mouse en el mundo con z = 0
    public static Vector3 GetMouseWorldPosition()
    {
        // Usando el nuevo Input System
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 screenPosition = new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0f);

        // Para 2D, necesitamos la distancia desde la c�mara al plano del juego
        Camera cam = Camera.main;
        if (cam.orthographic)
        {
            // Para ortogr�fica, la distancia z no afecta
            screenPosition.z = 0f;
        }
        else
        {
            // Para perspectiva, ponemos la distancia desde la c�mara al objeto
            screenPosition.z = cam.nearClipPlane + 0.1f;
        }

        Vector3 worldPosition = GetMouseWorldPositionWithZ(screenPosition, cam);
        worldPosition.z = 0f; // plano 2D
        return worldPosition;
    }
}
