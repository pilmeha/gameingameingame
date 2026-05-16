using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ScopeCursor : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rectTransform;
    private ScopeCanvas scopeCanvas;

    [SerializeField] private AudioSource shootSFX;

    void Awake()
    {
        scopeCanvas = GetComponentInParent<ScopeCanvas>();
        rectTransform = GetComponent<RectTransform>();
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
        shootSFX.Play();
    }
}
