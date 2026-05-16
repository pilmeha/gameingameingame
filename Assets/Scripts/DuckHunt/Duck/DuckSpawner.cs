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

    private WaitForSeconds spawnDelayCoroutine;
    [Header("Debug")]

    [SerializeField] private List<Duck> ducks;

    [SerializeField] private int currentDuckIndex = 0;

    void Awake()
    {
        spawnDelayCoroutine = new(defaultSpawnDelay);
        ducks = new(maxDucks);
        for (int i = 0; i < ducks.Capacity; i++)
        {
            Duck duck = Instantiate(duckPrefab, duckContainerTransform);
            duck.SetPath(path);
            duck.gameObject.SetActive(false);
            ducks.Add(duck);
        }
    }

    public void StartRound()
    {
        StartCoroutine(nameof(SpawnDucks));
    }

    public void SetDifficulty(DuckDiffuculty duckDifficulty)
    {
        
    }

    private IEnumerator SpawnDucks()
    {
        float spawnDelay = 0f;
        float spawnDelayDelta = defaultSpawnDelay * 0.5f;
        foreach (Duck duck in ducks)
        {
            duck.Activate();
            spawnDelay = UnityEngine.Random.Range(-spawnDelayDelta, spawnDelayDelta) + defaultSpawnDelay;
            spawnDelayCoroutine = new(spawnDelay);
            yield return spawnDelayCoroutine;
        }
    }
}
