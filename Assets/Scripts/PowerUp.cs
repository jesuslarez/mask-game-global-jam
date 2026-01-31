using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        SpeedBoost,      // Increases player speed
        Invulnerability  // Grants temporary invulnerability
    }

    public PowerUpType powerUpType; // Type of the power-up
    public Sprite speedBoostSprite; // Sprite for SpeedBoost
    public Sprite invulnerabilitySprite; // Sprite for Invulnerability

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the sprite based on the power-up type
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        switch (powerUpType)
        {
            case PowerUpType.SpeedBoost:
                spriteRenderer.sprite = speedBoostSprite;
                break;

            case PowerUpType.Invulnerability:
                spriteRenderer.sprite = invulnerabilitySprite;
                break;

            default:
                Debug.LogWarning("PowerUpType not handled: " + powerUpType);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Notify the player that they collected this power-up
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.CollectPowerUp(this);
                gameObject.SetActive(false); // Disable the power-up object
            }
        }
    }
}