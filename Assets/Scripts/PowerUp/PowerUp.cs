using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    public PowerUpDatabaseSO database;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Refresh();
    }

    public void SetType(PowerUpType type)
    {
        powerUpType = type;
        Refresh();
    }

    private void Refresh()
    {
        if (database == null) return;

        var def = database.Get(powerUpType);

        if (def != null)
            spriteRenderer.sprite = def.sprite;
    }
}

public enum PowerUpType
{
    SpeedBoost,      // Increases player speed
    Invulnerability,  // Grants temporary invulnerability
    Stun    // Freezes enemies in a surrounding area
}
