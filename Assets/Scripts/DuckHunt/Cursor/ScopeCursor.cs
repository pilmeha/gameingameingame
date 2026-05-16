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

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
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
    }
}
