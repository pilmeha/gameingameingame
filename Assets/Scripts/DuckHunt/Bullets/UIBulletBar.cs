using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIBulletBar : MonoBehaviour
{
    [SerializeField] private UIBullet bulletPrefab;

    [SerializeField] private List<UIBullet> bullets;
    
    void OnEnable()
    {
        DuckGameManager.Instance.OnShotFired.AddListener(UpdateBullets);
        DuckGameManager.Instance.OnReload.AddListener(Reload);
    }

    void OnDisable()
    {
        DuckGameManager.Instance.OnShotFired.RemoveListener(UpdateBullets);
        DuckGameManager.Instance.OnReload.RemoveListener(Reload);
    }

    void Reload(int reloads)
    {
        Unload();
        int maxBullets = DuckGameManager.Instance.MaxBulletCount;
        for (int i = 0; i < maxBullets; i++)
        {
            UIBullet bullet = Instantiate(bulletPrefab, transform);
            bullets.Add(bullet);
        }
    }

    void Unload()
    {
        foreach (Transform bullet in transform)
            Destroy(bullet.gameObject);

        bullets = new();
    }

    private void UpdateBullets(int bulletsLeft)
    {
        while (bullets.Count > bulletsLeft)
        {
            UIBullet bullet = bullets.First();
            bullets.Remove(bullet);
            Destroy(bullet.gameObject);
        }
    }
}
