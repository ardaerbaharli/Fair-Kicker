using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetsPanel : MonoBehaviour
{
    [SerializeField] private float slideDuration = 1f;
    [SerializeField] private GameObject goBackButton;
    [SerializeField] private RangeSlider slider;

    [SerializeField] private InputField rateInput;
    [SerializeField] private Text oddText;
    [SerializeField] private Text moneyText;


    private float odd;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        goBackButton.SetActive(true);
        goBackButton.GetComponent<Button>().onClick.AddListener(GoBack);
        ResetValues();
    }

    private void OnDisable()
    {
        goBackButton.GetComponent<Button>().onClick.RemoveListener(GoBack);
    }

    private void ResetValues()
    {
        rateInput.transform.Find("Placeholder").GetComponent<Text>().text = "0";
        slider.ResetSlider();
        SliderValueChanged();
    }


    public void SliderValueChanged()
    {
        // sliderKnob.sprite = Resources.Load<Sprite>("dot-" + (slider.value - 1));
        CalculateOdd();
    }

    private void CalculateOdd()
    {
        if (slider.maxLimit == slider.currentMaxValue && slider.currentMinValue == slider.minLimit)
            odd = 1;
        else
        odd = slider.maxLimit / (slider.currentMaxValue - slider.currentMinValue);
        
        oddText.text = odd.ToString("0.00");
    }

    public void BuyTimeButton()
    {
        var result = GameManager.Instance.BuyImprovement(GameManager.Improvement.Time);
        if (result)
        {
            moneyText.text = PlayerPrefs.GetInt("Money").ToString();
        }
    }

    public void BuyAccurateButton()
    {
        var result = GameManager.Instance.BuyImprovement(GameManager.Improvement.Accurate);
        if (result)
        {
            moneyText.text = PlayerPrefs.GetInt("Money").ToString();
        }
    }

    public void BuySpeedButton()
    {
        var result = GameManager.Instance.BuyImprovement(GameManager.Improvement.Speed);
        if (result)
        {
            moneyText.text = PlayerPrefs.GetInt("Money").ToString();
        }
    }


    public void PlayButton()
    {
        if (string.IsNullOrEmpty(rateInput.text)) return;
        var rate = int.Parse(rateInput.text);
        var money = PlayerPrefs.GetInt("Money", 0);
        if (rate > money) return;
        if (rate <= 0) return;
        GameManager.Instance.SetBet(rate, odd, (int) slider.currentMinValue, (int) slider.currentMaxValue);
        GameManager.Instance.Play();
    }

    private void GoBack()
    {
        goBackButton.SetActive(false);
        ArdaTween.MoveTo(gameObject, ArdaTween.Hash(
            "targetPosition", _startPosition,
            "duration", slideDuration,
            "setActive", false));
        // gameObject.SetActive(false);
    }
}