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
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }

    public void GoToOptions()
    {
        SceneManager.LoadScene("OptionsScene");
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
        GeneralAudioManager.Instance.StopClip();
        SceneManager.LoadScene("MenuScene");
    }
}
