using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    private SpawnController spawnController;
    private SpawnPoint point;
    private SpawnType type;
    public PowerUp powerUp;
    private bool pickupEnabled = true;
    
    public void Init(SpawnController spawnController, SpawnPoint point, SpawnType type)
    {
        this.spawnController = spawnController;
        this.point = point;
        this.type = type;
        if (type == SpawnType.Mask)
        {
            powerUp = GetComponent<PowerUp>();

            PowerUpType[] values = (PowerUpType[])System.Enum.GetValues(typeof(PowerUpType));

            PowerUpType randomType = values[Random.Range(0, values.Length)];

           powerUp.SetType(randomType);
             
        }
    }

    public void PickUp()
    {

        if (point != null) point.ClearIfCurrent(this);
        if (spawnController != null) spawnController.OnItemPicked(type, this);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (point != null) point.ClearIfCurrent(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pickupEnabled) return;
        if (!collision.CompareTag("Player")) return;

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) return;

        if (type == SpawnType.Key)
        {
            if (!player.hasKey)
            {
                player.CollectKey();
                PickUp();
            }
            return;
        }

        if (type == SpawnType.Mask)
        {
            if (powerUp != null)
            {
                player.CollectPowerUp(powerUp.powerUpType);
            }

            PickUp();
            return;
        }
    }

    public void DisablePickupFor(float seconds)
    {
        pickupEnabled = false;
        Invoke(nameof(EnablePickup), seconds);
    }

    private void EnablePickup() => pickupEnabled = true;

}
