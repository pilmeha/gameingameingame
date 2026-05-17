using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class ScopeCursor : MonoBehaviour
{
    private static readonly int ReloadHash = Animator.StringToHash("Reload");
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private RectTransform rectTransform;
    private Animator animator;

    [Header("Sound")]
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private AudioSource reloadSFX;
    private bool canShoot = true;
    private bool canReload = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        Cursor.visible = false;
    }

    void OnEnable()
    {
        DuckGameManager.Instance.OnShotFired.AddListener(CheckBullets);
        DuckGameManager.Instance.OnReload.AddListener(Reload);
    }

    void OnDisable()
    {
        DuckGameManager.Instance.OnShotFired.RemoveListener(CheckBullets);
        DuckGameManager.Instance.OnReload.RemoveListener(Reload);
    }

    void Update()
    {
        Move();

        if (!DuckGameManager.Instance.GameActive) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        if (canShoot)
            Shoot();
        else if (canReload)
            Reload();
        else
            DuckGameManager.Instance.AmmoDepleted();
        Debug.Log($"{name}: can shoot - {canShoot}; can reload - {canReload}");
    }

    void Move()
    {
        rectTransform.position = Mouse.current.position.ReadValue();
    }

    void Shoot()
    {
        animator.SetTrigger(ShootHash);
        shootSFX.Play();

        DuckGameManager.Instance.Shoot();
    }

    void Reload()
    {
        animator.SetTrigger(ReloadHash);
        reloadSFX.Play();

        DuckGameManager.Instance.Reload();
    }

    void CheckBullets(int bullets) => canShoot = bullets > 0;
    void Reload(int reloads)
    {   
        if (canReload && reloads >= 0)
            canShoot = true;

        canReload = reloads > 0;        
    }
}
