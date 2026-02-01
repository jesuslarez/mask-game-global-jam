using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class keyDoorArea : MonoBehaviour
{
    public int keyOnDoor;
    public Sprite[] doorSpritesArray;
    public UI_Key uiKey;

    public SpriteRenderer sr;
    private bool isOpen;
    private bool isFullyOpen;
    private PlayerController playerController;
    public AudioClip doorOpening;

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        sr.sprite = doorSpritesArray[0];
        isOpen = false;
        isFullyOpen = false;
    }

    public IEnumerator openDoor()
    {
        playerController.PlaySound(doorOpening);
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
        SceneManager.LoadScene("WinScene");
        Time.timeScale = 0;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isFullyOpen)
        {
            Win();
            return;
        }
    }
}
