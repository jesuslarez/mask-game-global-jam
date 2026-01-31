using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/PowerUp")]
public class PowerUpSO : ScriptableObject
{
    public PowerUpType type;
    public Sprite sprite;
    public string powerName;

    public float duration = 5f;

    // Stats(? TODO
    public float speedMultiplier = 3f;
}
