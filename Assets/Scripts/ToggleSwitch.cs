using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
{
    public bool isOn;

    [SerializeField] private Image switchBackground;
    [SerializeField] private Transform switchKnob;

    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    [Tooltip("Local position")] [SerializeField]
    private Vector3 knobOnPosition;

    [SerializeField] private Vector3 knobOffPosition;

    [SerializeField] private float colorChangeDuration;

    public delegate void ValueChanged(bool value);

    public event ValueChanged valueChanged;

    public void Toggle(bool value)
    {
        isOn = value;
        ToggleColor(isOn);

        if (valueChanged != null)
            valueChanged(isOn);
    }

    private void ToggleColor(bool value)
    {

        ArdaTween.ImageColorTo(switchBackground.gameObject,
            ArdaTween.Hash("image", switchBackground,
                "color", value ? onColor : offColor,
                "duration", colorChangeDuration));


        ArdaTween.MoveTo(switchKnob.gameObject,
            ArdaTween.Hash("targetPosition", value ? knobOnPosition : knobOffPosition,
                "duration", colorChangeDuration, "isLocalPosition", true));
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!isOn);
    }
}