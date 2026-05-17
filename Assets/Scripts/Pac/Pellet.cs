using UnityEngine;

public class Pellet : MonoBehaviour
{
    public bool isPowerPellet = false;   // 是否是超级豆
    private Vector3 spawnPosition;        // 自身位置

    private void Start()
    {
        spawnPosition = transform.position;
    }

    // 当豆子被销毁时（例如被玩家吃到），通知 Spawner 重生
    private void OnDestroy()
    {
        PelletSpawner spawner = FindObjectOfType<PelletSpawner>();
        if (spawner != null)
        {
            spawner.RespawnPellet(spawnPosition, isPowerPellet);
        }
    }
}