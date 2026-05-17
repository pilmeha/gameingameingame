using System;
using UnityEngine;
using UnityEngine.Events;

public class DuckGameManager : MonoBehaviour
{
    public static DuckGameManager Instance;
    [SerializeField] private DuckSpawner duckSpawner;
    [SerializeField] private Dog dog;

    private float score = 0f;
    private int duckMissed = 0;
    private int duckShot = 0;
    [SerializeField] private int maxBulletCount = 6;
    public int MaxBulletCount { get => maxBulletCount; }
    [SerializeField] private int maxReloads = 1;
    public int MaxReloads { get => maxReloads; }

    private bool hasAmmo = true;
    public bool HasAmmo { get => hasAmmo; }
    private int bulletCount = 6;
    private int reloads = 1;
    [HideInInspector] public UnityEvent<float> OnScoreChange = new();
    [HideInInspector] public UnityEvent<int> OnShotFired = new();
    [HideInInspector] public UnityEvent<int> OnReload = new();
    [HideInInspector] public UnityEvent<int> OnDuckCountChanged = new();
    [HideInInspector] public UnityEvent OnAmmoDepleted = new();


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
        if (duckSpawner == null)
            duckSpawner = FindFirstObjectByType<DuckSpawner>();
    }

    void Start()
    {
        reloads = maxReloads + 1;
        duckMissed = duckSpawner.MaxDucks;
        duckShot = 0;
        Debug.Log($"<color=yellow>{name}:</color> Initial reloading...");
        Reload();
        duckSpawner.StartRound();
    }

    public void AddScore(float deltaScore)
    {
        if (deltaScore < 0)
            return;
        score += deltaScore;
        duckShot++;
        duckMissed--;
        OnScoreChange.Invoke(score);
        OnDuckCountChanged.Invoke(duckShot);
    }

    public void Shoot()
    {
        if (bulletCount < 0){
            hasAmmo = false;
            return;
        }
        
        bulletCount--;        
        OnShotFired.Invoke(bulletCount);
    }

    public void Reload()
    {
        if (reloads <= 0)
            return;
        Debug.Log($"<color=yellow>{name}:</color> Reloading...");
        bulletCount = maxBulletCount;
        hasAmmo = true;
        reloads--;        
        OnReload.Invoke(reloads);
    }

    public void AmmoDepleted()
    {
        Debug.Log($"<color=yellow>{name}:</color> Ammo depleted... Stopping");
        hasAmmo = false;
        OnAmmoDepleted.Invoke();
    }

    public DuckGameResults GetResults()
    {
        DuckGameResults results = new()
        {
            DogShot = dog.Status == DuckStatus.Damaged,
            DuckMissed = duckMissed,
            DuckShot = duckShot,
            Score = score
        };
        return results;
    }
    public int GetDuckCount()
    {
        return duckSpawner.MaxDucks;
    }
}
