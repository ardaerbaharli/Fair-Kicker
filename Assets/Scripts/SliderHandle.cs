using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderHandle : MonoBehaviour
{
    public InputHandler inputHandler;
    public Vector2 beginPosition, endPosition;
    public Image handleImage;
    public float sliderLeftBoundX, sliderRightBoundX;
    [SerializeField] private RectTransform rect;
    public float size;

    public float value;

    public delegate void HandleValueChanged(float deltaPosFromStart);

    public event HandleValueChanged handleValueChanged;

    private void Awake()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        size = rect.sizeDelta.x;
        beginPosition = rect.anchoredPosition;

        inputHandler.onEndDrag_ += OnEndDrag;
        inputHandler.onDrag_ += OnDrag;
        
    }

    private void OnDrag(PointerEventData data)
    {
        var deltaP = new Vector2(data.delta.x, 0);
        var pivotPos = rect.anchoredPosition + deltaP;
        pivotPos.x = Mathf.Clamp(pivotPos.x, sliderLeftBoundX, sliderRightBoundX);
        rect.anchoredPosition = pivotPos;
        handleValueChanged?.Invoke(Mathf.Abs(rect.anchoredPosition.x - beginPosition.x));
    }

    private void OnEndDrag()
    {
        endPosition = rect.anchoredPosition;
        handleValueChanged?.Invoke(Mathf.Abs(rect.anchoredPosition.x - beginPosition.x));
    }

    public Vector2 GetPosition()
    {
        return rect.anchoredPosition;
    }

    public void SetValue(float currentMaxValue)
    {
        value = currentMaxValue;
    }

    public void SetValue(int currentValue)
    {
        value = currentValue;
        handleImage.sprite = Resources.Load<Sprite>("dot-" + (value - 1));
    }
}