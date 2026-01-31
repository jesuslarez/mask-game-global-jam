using UnityEngine;

public class MainDoorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int keyOnDoor;
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) return;


        if (player.hasKey)
        {
            keyOnDoor++;
            player.hasKey = false;

        }
        player.UseKey();


    
    }
}
