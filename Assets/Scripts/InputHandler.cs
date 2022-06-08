using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler,
    IPointerUpHandler
{
    public delegate void OnBeginDrag_();

    public event OnBeginDrag_ onBeginDrag_;

    public delegate void OnDrag_(PointerEventData data);

    public event OnDrag_ onDrag_;

    public delegate void OnEndDrag_();

    public event OnEndDrag_ onEndDrag_;

    public delegate void OnPointerDown_();

    public event OnPointerDown_ onPointerDown_;

    public delegate void OnPointerUp_();

    public event OnPointerUp_ onPointerUp_;


    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag_?.Invoke();
    }

    public void OnDrag(PointerEventData data)
    {
        onDrag_?.Invoke(data);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag_?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown_?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp_?.Invoke();
    }
}