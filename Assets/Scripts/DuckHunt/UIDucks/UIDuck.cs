using UnityEngine;
using UnityEngine.UI;

public class UIDuck : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite missed;
    [SerializeField] private Sprite shot;

    private DuckStatus status;

    public void SetStatus(DuckStatus s)
    {
        status = s;
        switch (status)
        {
            case DuckStatus.Alive:
                image.sprite = missed;
                break;
            case DuckStatus.Damaged:
                image.sprite = shot;
                break;
        }
    }
}
