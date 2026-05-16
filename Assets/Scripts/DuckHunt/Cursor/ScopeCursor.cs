using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ScopeCursor : MonoBehaviour, IPointerClickHandler
{
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private RectTransform rectTransform;
    private ScopeCanvas scopeCanvas;
    private Animator animator;

    [SerializeField] private AudioSource shootSFX;

    void Awake()
    {
        scopeCanvas = GetComponentInParent<ScopeCanvas>();
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        Cursor.visible = false;
    }

    void OnEnable()
    {
        scopeCanvas.OnCursorMove.AddListener(Move);
    }

    void OnDisable()
    {
        scopeCanvas.OnCursorMove.RemoveListener(Move);
    }

    void Move(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        animator.SetTrigger(ShootHash);
        shootSFX.Play();
    }
}
