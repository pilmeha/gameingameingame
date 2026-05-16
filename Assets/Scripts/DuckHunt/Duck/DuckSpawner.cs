using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class DuckSpawner : MonoBehaviour
{
    [SerializeField] private Duck duckPrefab;
    [SerializeField] private Transform duckContainerTransform;
    [SerializeField] private SplineContainer path;

    [SerializeField] private int maxDucks = 8;
    private float defaultSpawnDelay = 5f;

    private DuckDiffuculty difficulty = DuckDiffuculty.Low;

    private WaitForSeconds spawnDelayCoroutine;
    [Header("Debug")]

    [SerializeField] private List<Duck> ducks;

    void Awake()
    {
        spawnDelayCoroutine = new(defaultSpawnDelay);
        ducks = new(maxDucks);
        
    }

    public void StartRound()
    {
        StartCoroutine(nameof(SpawnDucks));
    }

    public void SetDifficulty(DuckDiffuculty duckDifficulty)
    {
        difficulty = duckDifficulty;
    }

    private Duck SpawnDuck(DuckDiffuculty difficulty = DuckDiffuculty.Low)
    {
        Duck duck = Instantiate(duckPrefab, duckContainerTransform);
        duck.SetPath(path);
        duck.SetDifficulty(difficulty);
        ducks.Add(duck);
        return duck;
    }

    private IEnumerator SpawnDucks()
    {
        float spawnDelay = 0f;
        float spawnDelayDelta = defaultSpawnDelay * 0.5f;
        for (int i = 0;i < maxDucks; i++)
        {
            Duck duck = SpawnDuck(difficulty);
            duck.Activate();
            spawnDelay = UnityEngine.Random.Range(-spawnDelayDelta, spawnDelayDelta) + defaultSpawnDelay;
            spawnDelayCoroutine = new(spawnDelay);
            yield return spawnDelayCoroutine;
        }
    }
}
