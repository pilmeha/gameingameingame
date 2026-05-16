using System;
using UnityEngine;
using UnityEngine.Events;

public class DuckGameManager : MonoBehaviour
{
    public static DuckGameManager Instance;
    [SerializeField] private DuckSpawner duckSpawner;

    private float score = 0f;
    public UnityEvent<float> OnScoreChange = new();
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        if (duckSpawner == null)
            duckSpawner = FindFirstObjectByType<DuckSpawner>();
    }

    void Start()
    {
        duckSpawner.StartRound();
    }

    public void AddScore(float deltaScore)
    {
        if (deltaScore < 0)
            return;
        score += deltaScore;
        OnScoreChange.Invoke(score);
    }
}
