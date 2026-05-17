using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PacManGameManager : MonoBehaviour
{
    public Text ScoreText;
    public GameObject floatingTextPrefab;

    public GameObject[] ghostPrefabs;
    public Transform[] ghostSpawnPoints;

    private int score = 0;
    private int ghostEatCount = 0;
    private bool isPowerMode = false;
    private float powerTimer = 0f;
    private int[] ghostScores = { 100, 300, 500, 700, 1000, 2000, 3000, 5000 };

    // 维护每个鬼的重生请求（协程）
    private bool[] respawnPending;            // 是否正在等待重生
    private Coroutine[] respawnCoroutines;

    void Start()
    {
        score = 0;
        UpdateUI();
        SpawnAllGhosts();
    }

    void Update()
    {
        if (isPowerMode)
        {
            powerTimer -= Time.deltaTime;
            if (powerTimer <= 0f)
                DisablePowerMode();
        }
    }

    void SpawnAllGhosts()
    {
        // 初始化数组大小
        respawnPending = new bool[ghostPrefabs.Length];
        respawnCoroutines = new Coroutine[ghostPrefabs.Length];

        for (int i = 0; i < ghostPrefabs.Length; i++)
        {
            SpawnGhostAtIndex(i);
        }
    }

    void SpawnGhostAtIndex(int index)
    {
        if (index >= ghostPrefabs.Length || index >= ghostSpawnPoints.Length)
            return;

        GameObject ghostObj = Instantiate(ghostPrefabs[index], ghostSpawnPoints[index].position, Quaternion.identity);
        GhostGridMover ghost = ghostObj.GetComponent<GhostGridMover>();
        if (ghost != null)
        {
            ghost.InitializeGhost(ghostSpawnPoints[index].position, index);
        }
        else
        {
            Debug.LogError($"预制体 {ghostPrefabs[index].name} 缺少 GhostGridMover 组件");
        }
    }

    // 由幽灵在销毁前调用，请求重生
    public void RequeueGhostRespawn(int ghostIndex, Vector2 spawnPos)
    {
        if (ghostIndex < 0 || ghostIndex >= ghostPrefabs.Length) return;

        // 如果已经有等待中的重生，先取消旧的
        if (respawnPending[ghostIndex])
        {
            if (respawnCoroutines[ghostIndex] != null)
                StopCoroutine(respawnCoroutines[ghostIndex]);
        }

        // 开始新重生协程
        respawnCoroutines[ghostIndex] = StartCoroutine(RespawnGhostAfterDelay(ghostIndex));
    }

    IEnumerator RespawnGhostAfterDelay(int index)
    {
        respawnPending[index] = true;
        // 经典吃豆人重生延迟（比如3秒），可调整
        yield return new WaitForSeconds(3f);
        SpawnGhostAtIndex(index);
        respawnPending[index] = false;
        respawnCoroutines[index] = null;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
        if (GameObject.FindGameObjectsWithTag("Pellet").Length == 0 &&
            GameObject.FindGameObjectsWithTag("PowerPellet").Length == 0)
            WinGame();
    }

    void UpdateUI()
    {
        ScoreText.text = "SCORE: " + score.ToString("D4");
    }

    public void PowerUp()
    {
        isPowerMode = true;
        powerTimer = 8f;
        ghostEatCount = 0;

        GhostGridMover[] ghosts = FindObjectsOfType<GhostGridMover>();
        foreach (var ghost in ghosts)
            ghost.MakeVulnerable();
    }

    void DisablePowerMode()
    {
        isPowerMode = false;
        foreach (var ghost in FindObjectsOfType<GhostGridMover>())
            ghost.MakeNormal();
    }

    // 被吃鬼的加分（位置版本，用于已经销毁的幽灵）
    public void EatGhostAtPosition(Vector3 ghostPosition)
    {
        if (!isPowerMode) return;
        int index = Mathf.Min(ghostEatCount, ghostScores.Length - 1);
        int points = ghostScores[index];
        score += points;
        UpdateUI();
        ShowFloatingText(ghostPosition, points);
        ghostEatCount++;
    }

    void ShowFloatingText(Vector3 pos, int value)
    {
        if (floatingTextPrefab == null)
        {
            Debug.LogWarning("floatingTextPrefab 未赋值，无法显示浮动数字");
            return;
        }
        GameObject obj = Instantiate(floatingTextPrefab, pos, Quaternion.identity);
        TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = value.ToString();
        else
            obj.GetComponent<Text>().text = value.ToString();
        Destroy(obj, 1f);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }

    void WinGame()
    {
        Debug.Log("You Win!");
        Time.timeScale = 0;
    }
}