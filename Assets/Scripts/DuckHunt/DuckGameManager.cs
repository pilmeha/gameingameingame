using System;
using UnityEngine;
using UnityEngine.Events;

public class DuckGameManager : MonoBehaviour
{
    public static DuckGameManager Instance;
    [SerializeField] private DuckSpawner duckSpawner;

    private float score = 0f;
    private int maxBulletCount = 6;
    public int MaxBulletCount {get => maxBulletCount;}
    private int bulletCount = 6;
    public UnityEvent<float> OnScoreChange = new();
    public UnityEvent<int> OnShotFired = new();
    
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
        if (bulletCount > maxBulletCount)
            bulletCount = maxBulletCount;
        duckSpawner.StartRound();
    }

    public void AddScore(float deltaScore)
    {
        if (deltaScore < 0)
            return;
        score += deltaScore;
        OnScoreChange.Invoke(score);
    }

    public void Shot()
    {
        if (bulletCount < 0)
            return;
        bulletCount--;
        OnShotFired.Invoke(bulletCount);
    }
}
