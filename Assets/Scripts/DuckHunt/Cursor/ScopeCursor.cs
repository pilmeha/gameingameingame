using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class ScopeCursor : MonoBehaviour
{
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private RectTransform rectTransform;
    private Animator animator;

    [SerializeField] private AudioSource shootSFX;
    private bool canShoot = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        Cursor.visible = false;
    }

    void OnEnable() => DuckGameManager.Instance.OnShotFired.AddListener(CheckBullets);
    void OnDisable() => DuckGameManager.Instance.OnShotFired.RemoveListener(CheckBullets);

    void Update()
    {
        Move();
        
        if (canShoot && Mouse.current.leftButton.wasPressedThisFrame)
            Click();
    }

    void Move()
    {
        rectTransform.position = Mouse.current.position.ReadValue();
    }

    void Click()
    {
        animator.SetTrigger(ShootHash);
        shootSFX.Play();
        
        DuckGameManager.Instance.Shot();
    }

    void CheckBullets(int current)
    {
        canShoot = current > 0;
    }
}
