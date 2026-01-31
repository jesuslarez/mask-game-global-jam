using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        SpeedBoost, // Increases player speed
        // Add more power-up types here
    }

    public PowerUpType powerUpType; // Type of the power-up

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