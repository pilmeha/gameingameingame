using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ScopeCursor : MonoBehaviour
{
    private RectTransform rectTransform;
    private ScopeCanvas scopeCanvas;

    void Awake()
    {
        scopeCanvas = GetComponentInParent<ScopeCanvas>();
        rectTransform = GetComponent<RectTransform>();
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
}
