using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PelletSpawner : MonoBehaviour
{
    public Tilemap wallTilemap;          // 墙壁 Tilemap（用来判断哪些格子不可走）
    public GameObject pelletPrefab;      // 普通豆子预制体（需挂载 Pellet 脚本）
    public GameObject powerPelletPrefab; // 超级豆预制体（需挂载 Pellet 脚本）
    public Transform pelletsContainer;   // 可选：存放豆子的父物体

    public BoundsInt mazeBounds;

    private void Start()
    {
        SpawnAllPellets();
    }

    // 初始生成所有豆子
    private void SpawnAllPellets()
    {
        if (wallTilemap == null)
        {
            Debug.LogError("未指定 wallTilemap！");
            return;
        }

        if (mazeBounds == new BoundsInt())
        {
            mazeBounds = wallTilemap.cellBounds;
        }

        int xMin = mazeBounds.xMin;
        int xMax = mazeBounds.xMax;
        int yMin = mazeBounds.yMin;
        int yMax = mazeBounds.yMax;

        for (int x = xMin; x < xMax; x++)
        {
            for (int y = yMin; y < yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (wallTilemap.GetTile(cell) != null)
                    continue;

                Vector3 worldPos = wallTilemap.GetCellCenterWorld(cell);

                // 判断是否为四个角落 -> 生成超级豆
                bool isCorner = (x == xMin || x == xMax - 1) && (y == yMin || y == yMax - 1);

                GameObject prefab = (isCorner && powerPelletPrefab != null) ? powerPelletPrefab : pelletPrefab;
                GameObject pellet = Instantiate(prefab, worldPos, Quaternion.identity);

                // 确保 Pellet 脚本存在，并设置是否为超级豆
                Pellet pelletScript = pellet.GetComponent<Pellet>();
                if (pelletScript != null)
                    pelletScript.isPowerPellet = isCorner;

                if (pelletsContainer != null)
                    pellet.transform.SetParent(pelletsContainer);
            }
        }
    }

    // 公开方法：延迟重生豆子
    public void RespawnPellet(Vector3 position, bool isPowerPellet)
    {
        StartCoroutine(RespawnCoroutine(position, isPowerPellet));
    }

    private IEnumerator RespawnCoroutine(Vector3 position, bool isPowerPellet)
    {
        yield return new WaitForSeconds(20f);

        // 确定要生成的预制体
        GameObject prefab = isPowerPellet ? powerPelletPrefab : pelletPrefab;
        if (prefab == null)
        {
            Debug.LogError($"重生失败：{(isPowerPellet ? "超级豆" : "普通豆")} 预制体未指定");
            yield break;
        }

        // 生成新豆子
        GameObject newPellet = Instantiate(prefab, position, Quaternion.identity);

        // 重新设置 Pellet 脚本中的标志
        Pellet pelletScript = newPellet.GetComponent<Pellet>();
        if (pelletScript != null)
            pelletScript.isPowerPellet = isPowerPellet;

        if (pelletsContainer != null)
            newPellet.transform.SetParent(pelletsContainer);
    }
}