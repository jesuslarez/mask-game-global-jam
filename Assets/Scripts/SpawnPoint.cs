using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private SpawnType type;

    public SpawnType Type => type;
    public bool IsEmpty => current == null;

    private ItemSpawn current;

    public ItemSpawn Spawn(GameObject prefab, SpawnController spawnController)
    {
        if (!(current == null) || prefab == null) return null;

        var go = Instantiate(prefab, transform.position, Quaternion.identity);
        var item = go.GetComponent<ItemSpawn>();
        current = item;
        item.Init(spawnController, this, type);
        return item;
    }

    public void ClearIfCurrent(ItemSpawn item)
    {
        if (current == item) current = null;
    }
}
