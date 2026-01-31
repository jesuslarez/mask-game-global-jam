using UnityEngine;

public class PowerUp : MonoBehaviour
{
   

    public PowerUpType powerUpType; // Type of the power-up

   

    public void SetType(PowerUpType type)
    {
        powerUpType = type;
         
    }
}
public enum PowerUpType
{
    SpeedBoost, // Increases player speed
                // Add more power-up types here
}