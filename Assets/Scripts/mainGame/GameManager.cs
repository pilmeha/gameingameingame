using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Keys")]
    public bool duckHuntKey;
    public bool zeldaKey;
    public bool pacmanKey;

    [Header("Scenes")]
    public string hubSceneName = "MainHub";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}