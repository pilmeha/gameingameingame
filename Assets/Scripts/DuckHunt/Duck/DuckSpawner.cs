using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class DuckSpawner : MonoBehaviour
{
    [SerializeField] private Duck duckPrefab;
    [SerializeField] private Transform duckContainerTransform;
    [SerializeField] private SplineContainer path;

    [SerializeField] private int maxDucks = 10;
    private int activeDucks = 0;
    public int MaxDucks { get => maxDucks; }
    private float defaultSpawnDelay = 3f;

    private DuckDiffuculty difficulty = DuckDiffuculty.Low;

    private Coroutine spawnCoroutine;

    private WaitForSeconds spawnDelayCoroutine;

    public UnityEvent OnRoundEnd = new();
    [Header("Debug")]

    [SerializeField] private List<Duck> ducks;

    void Awake()
    {
        spawnDelayCoroutine = new(defaultSpawnDelay);
        ducks = new(maxDucks);
    }

    void OnEnable()
    {
        DuckGameManager.Instance.OnAmmoDepleted.AddListener(StopRound);
    }

    void OnDisable()
    {
        DuckGameManager.Instance.OnAmmoDepleted.RemoveListener(StopRound);
    }

    void StopRound()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
        OnRoundEnd.Invoke();
    }

    public void StartRound()
    {
        spawnCoroutine = StartCoroutine(nameof(SpawnDucks));
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
        duck.OnDeactivate.AddListener(() => activeDucks--);
        ducks.Add(duck);
        return duck;
    }

    private IEnumerator SpawnDucks()
    {
        float spawnDelay;
        float spawnDelayDelta = defaultSpawnDelay * 0.5f;
        for (int i = 0; i < maxDucks; i++)
        {
            Duck duck = SpawnDuck(difficulty);
            duck.SetIndex(i);
            duck.Activate();
            activeDucks++;
            spawnDelay = UnityEngine.Random.Range(-spawnDelayDelta, spawnDelayDelta) + defaultSpawnDelay;
            spawnDelayCoroutine = new(spawnDelay);
            yield return spawnDelayCoroutine;
        }
        while (activeDucks > 0)
        {
            yield return new WaitForSeconds(0.2f);
        }
        OnRoundEnd.Invoke();
    }
}
