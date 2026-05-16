using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ScopeCanvas : MonoBehaviour, IPointerMoveHandler
{
    [HideInInspector] public UnityEvent<PointerEventData> OnCursorMove = new();
    public void OnPointerMove(PointerEventData eventData)
    {
        Debug.Log($"Cursor moved in canvas: {eventData.position}");
        OnCursorMove.Invoke(eventData);
    }
}
