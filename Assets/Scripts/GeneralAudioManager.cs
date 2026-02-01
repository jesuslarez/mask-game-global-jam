using Unity.VisualScripting;
using UnityEngine;

public class GeneralAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip winMusic;


    private AudioSource source;

    public static GeneralAudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void StopClip()
    {
        source.Stop();
    }

    public void PlayWinMusic()
    {
        source.clip = winMusic;
        source.Play();
    }
}
