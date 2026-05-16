using UnityEngine;

public class DuckGameManager : MonoBehaviour
{
    public static DuckGameManager Instance;
    [SerializeField] private DuckSpawner duckSpawner;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        if (duckSpawner == null)
            duckSpawner = FindFirstObjectByType<DuckSpawner>();
    }

    void Start()
    {
        duckSpawner.StartRound();
    }
}
