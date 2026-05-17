using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Keys")]
    public bool duckKey;
    public bool zeldaKey;
    public bool pacmanKey;

    [Header("Scenes")]
    public string hubSceneName = "mainGame";

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

    public int GetKeyCount()
    {
        int count = 0;

        if (duckKey) count++;
        if (zeldaKey) count++;
        if (pacmanKey) count++;

        return count;
    }

    public bool AllKeysCollected()
    {
        //return duckKey && zeldaKey && pacmanKey;
        return duckKey && zeldaKey;
    }

    public void GiveDuckKey()
    {
        SoundManager.Instance.PlayKeyPickup();
        duckKey = true;
    }

    public void GiveZeldaKey()
    {
        SoundManager.Instance.PlayKeyPickup();
        zeldaKey = true;
    }

    public void GivePacmanKey()
    {
        SoundManager.Instance.PlayKeyPickup();
        pacmanKey = true;
    }
}