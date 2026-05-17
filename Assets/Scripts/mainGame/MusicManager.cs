using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Main Hub")]
    [SerializeField] private AudioClip hubMusic;

    [Header("Mini Games")]
    [SerializeField] private AudioClip zeldaMusic;

    private AudioSource audioSource;

    private float hubMusicTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            PlayHubMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHubMusic()
    {
        audioSource.clip = hubMusic;

        audioSource.time = hubMusicTime;

        audioSource.Play();
    }

    public void PauseHubMusic()
    {
        hubMusicTime = audioSource.time;

        audioSource.Stop();
    }

    public void PlayZeldaMusic()
    {
        audioSource.clip = zeldaMusic;

        audioSource.time = 0f;

        audioSource.Play();
    }
}