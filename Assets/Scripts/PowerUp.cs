using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public PowerUpType powerUpType; // Type of the power-up
    public Sprite speedBoostSprite; // Sprite for SpeedBoost
    public Sprite invulnerabilitySprite; // Sprite for Invulnerability
    public Sprite stunSprite; // Sprite for Stun

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

            case PowerUpType.Stun:
                spriteRenderer.sprite = stunSprite;
                break;

            default:
                Debug.LogWarning("PowerUpType not handled: " + powerUpType);
                break;
        }
    }

   

    public void SetType(PowerUpType type)
    {
        powerUpType = type;
         
    }
}
public enum PowerUpType
{
    SpeedBoost,      // Increases player speed
    Invulnerability,  // Grants temporary invulnerability
    Stun, // Freezes enemies in a surrounding area
}
