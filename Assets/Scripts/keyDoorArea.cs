using UnityEngine;
using System.Collections;

public class keyDoorArea : MonoBehaviour
{
    public int keyOnDoor;
    public Sprite[] doorSpritesArray;
    public UI_Key uiKey;

    public SpriteRenderer sr;
    private bool isOpen;
    private bool isFullyOpen;

    void Start()
    {
        
        sr.sprite = doorSpritesArray[0];
        isOpen = false;
        isFullyOpen = false;
    }

    public IEnumerator openDoor()
    {
        uiKey.Opening();
        if (isOpen || isFullyOpen) yield break;

        isOpen = true;

        float stepTime = 5f / doorSpritesArray.Length;

        for (int i = 0; i < doorSpritesArray.Length; i++)
        {
            sr.sprite = doorSpritesArray[i];
            yield return new WaitForSeconds(stepTime);
        }

        isOpen = false;

        isFullyOpen = true;
        uiKey.isOpened();
    }

    private void Win()
    {
        Debug.Log("Win");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) return;

        if (isFullyOpen)
        {
            Win();
            return;
        }

        if (player.hasKey)
        {
            keyOnDoor++;
            player.hasKey = false;
            player.UseKey();
        }

        if (keyOnDoor == 3)
        {
            StartCoroutine(openDoor());
        }
    }
}
