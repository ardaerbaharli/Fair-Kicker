using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RangeSlider : MonoBehaviour
{
    private enum DragState
    {
        Both,
        Min,
        Max
    }

    [Header("Limits")] [SerializeField] public float minLimit = 0;
    [SerializeField] public float maxLimit = 100;

    [SerializeField] private SliderHandle minHandle;
    [SerializeField] private SliderHandle maxHandle;


    [SerializeField] private GameObject parentObject;
    [SerializeField] private float parentWidth;

    [SerializeField] private Image selectedRangeImage;

    [Header("Values")] public bool wholeNumbers;

    [SerializeField] public float currentMinValue;
    [SerializeField] public float currentMaxValue;


    private float stepSize;

    private Vector2 leftBound, rightBound;

    private void Start()
    {
        minHandle.handleValueChanged += OnMinHandleValueChanged;
        maxHandle.handleValueChanged += OnMaxHandleValueChanged;

        var sliderRect = parentObject.GetComponent<RectTransform>().rect;
        parentWidth = sliderRect.width;
        leftBound = new Vector2(sliderRect.xMin, sliderRect.y);
        rightBound = new Vector2(sliderRect.xMax, sliderRect.y);
        stepSize = (rightBound.x - leftBound.x) / (maxLimit - minLimit);

        minHandle.sliderLeftBoundX = leftBound.x;
        maxHandle.sliderRightBoundX = rightBound.x;

        minHandle.sliderRightBoundX = maxHandle.GetPosition().x - minHandle.size;
        maxHandle.sliderLeftBoundX = minHandle.GetPosition().x + minHandle.size;
        SetFillImage();
    }

    private void OnMinHandleValueChanged(float deltaPosFromStart)
    {
        currentMinValue = minLimit + Mathf.Clamp(deltaPosFromStart / stepSize, minLimit - 1, maxLimit - 1);


        SetFillImage();

        if (wholeNumbers)
        {
            currentMinValue = Mathf.Round(currentMinValue);
            minHandle.SetValue((int) currentMinValue);
        }
        else
        {
            minHandle.SetValue(currentMinValue);
        }

        minHandle.sliderRightBoundX = maxHandle.GetPosition().x - minHandle.size;
        onValueChanged?.Invoke(currentMinValue, currentMaxValue);

    }

    private void SetFillImage()
    {
        var size = -parentWidth + Mathf.Abs(maxHandle.GetPosition().x - minHandle.GetPosition().x);
        var pos = (maxHandle.GetPosition().x + minHandle.GetPosition().x) / 2;
        selectedRangeImage.rectTransform.anchoredPosition = new Vector2(pos, 0);
        selectedRangeImage.rectTransform.sizeDelta = new Vector2(size, selectedRangeImage.rectTransform.sizeDelta.y);
    }

    private void OnMaxHandleValueChanged(float deltaPosFromStart)
    {
        currentMaxValue = maxLimit - Mathf.Clamp(deltaPosFromStart / stepSize, minLimit - 1, maxLimit - 1);

        SetFillImage();

        if (wholeNumbers)
        {
            currentMaxValue = Mathf.Round(currentMaxValue);
            maxHandle.SetValue((int) currentMaxValue);
        }
        else
        {
            maxHandle.SetValue(currentMaxValue);
        }

        maxHandle.sliderLeftBoundX = minHandle.GetPosition().x + maxHandle.size;
        onValueChanged?.Invoke(currentMinValue, currentMaxValue);
    }

    [Serializable]
    public class SliderEvent : UnityEvent<float, float>
    {
    }

    public SliderEvent onValueChanged = new SliderEvent();

    public void ResetSlider()
    {
        currentMinValue = minLimit;
        currentMaxValue = maxLimit;
        SetFillImage();
    }
}