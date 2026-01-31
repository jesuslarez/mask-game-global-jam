using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public Vector3 position;

    private void Start()
    {
        position = transform.position;
    }
}
