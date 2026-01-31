using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    private SpawnController spawnController;
    private SpawnPoint point;
    private SpawnType type;

    public void Init(SpawnController spawnController, SpawnPoint point, SpawnType type)
    {
        this.spawnController = spawnController;
        this.point = point;
        this.type = type;
    }

    public void PickUp()
    {
        if (point != null) point.ClearIfCurrent(this);
        if (spawnController != null) spawnController.OnItemPicked(type);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (point != null) point.ClearIfCurrent(this);
    }
}
