using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            StartGame();
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            EndGame();
    }


    public void StartGame()
    {
        SceneManager.LoadScene("StartLetter");
        Time.timeScale = 1;
    }

    public void GoToHowToPlay()
    {
        SceneManager.LoadScene("HowToPlayScene");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void EndGame()
    {

        Application.Quit();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
