using System.Collections.Generic;
using UnityEngine;

public class UIDuckBar : MonoBehaviour
{
    [SerializeField] private UIDuck duckPrefab;
    [SerializeField] private List<UIDuck> ducks;

    void Start()
    {
        Clear();
    }

    void OnEnable()
    {
        DuckGameManager.Instance.OnDuckCountChanged.AddListener(UpdateDucks);
    }

    void OnDisable()
    {
        DuckGameManager.Instance.OnDuckCountChanged.RemoveListener(UpdateDucks);
    }

    void Clear()
    {
        Unload();
        int maxDucks = DuckGameManager.Instance.GetDuckCount();
        for (int i = 0; i < maxDucks; i++)
        {
            UIDuck bullet = Instantiate(duckPrefab, transform);
            ducks.Add(bullet);
        }
    }

    void Unload()
    {
        foreach (Transform bullet in transform)
            Destroy(bullet.gameObject);

        ducks = new();
    }

    private void UpdateDucks(int ducksShot)
    {
        int needToUpdate = ducksShot;
        foreach(UIDuck duck in ducks)
        {
            if (needToUpdate <= 0)
                break;
            
            duck.SetStatus(DuckStatus.Damaged);
            needToUpdate--;
        }
    }
}
