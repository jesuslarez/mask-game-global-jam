using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    
    [SerializeField] private GameObject maskPrefab;
    [SerializeField] private GameObject keyPrefab;

   
    [Min(0)] [SerializeField] private int maskActiveCount = 3; 
    [Min(0)] [SerializeField] private int keyStartCount = 3;   

    
    [Min(0f)] [SerializeField] private float maskRespawnDelay = 5f;

    private List<SpawnPoint> maskPoints = new();
    private List<SpawnPoint> keyPoints = new();

    private int masksActive;
    private bool started;

    void Awake()
    {
        var points = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        maskPoints = points.Where(p => p.Type == SpawnType.Mask).ToList();
        keyPoints = points.Where(p => p.Type == SpawnType.Key).ToList();
    }

    void Start()
    {
        started = true;

       SpawnNAtRandomEmpty(keyPoints, keyPrefab, keyStartCount);

        FillMasksToTarget();
    }

    public void OnItemPicked(SpawnType type)
    {
        if (!started) return;

        if (type == SpawnType.Key)
        {
            return;
        }

        if (type == SpawnType.Mask)
        {
            masksActive = Mathf.Max(0, masksActive - 1);
            StartCoroutine(RespawnMaskAfterDelay());
        }
    }

    private IEnumerator RespawnMaskAfterDelay()
    {
        yield return new WaitForSeconds(maskRespawnDelay);
        FillMasksToTarget();
    }

    private void FillMasksToTarget()
    {
        while (masksActive < maskActiveCount)
        {
            var spawned = SpawnOneAtRandomEmpty(maskPoints, maskPrefab);
            if (!spawned) break;
            masksActive++;
        }
    }

    private void SpawnNAtRandomEmpty(List<SpawnPoint> points, GameObject prefab, int n)
    {
        for (int i = 0; i < n; i++)
        {
            if (!SpawnOneAtRandomEmpty(points, prefab)) break;
        }
    }

    private bool SpawnOneAtRandomEmpty(List<SpawnPoint> points, GameObject prefab)
    {
        if (prefab == null || points == null || points.Count == 0) return false;

        var empty = points.Where(p => p.IsEmpty).ToList();
        if (empty.Count == 0) return false;

        var chosen = empty[Random.Range(0, empty.Count)];
        chosen.Spawn(prefab, this);
        return true;
    }
}

public enum SpawnType
{
    Mask,
    Key
}