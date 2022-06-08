using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private ToggleSwitch soundToggle;
    [SerializeField] private float slideDuration = 1f;
    [SerializeField] private GameObject goBackButton;
    [SerializeField] private AudioMixer mixer;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
        
        soundToggle.valueChanged += SoundToggleValueChanged;
        var soundValue = PlayerPrefs.GetInt("Sound", 1) == 1;
        soundToggle.Toggle(soundValue);
    }

    private void OnEnable()
    {
        goBackButton.SetActive(true);
        goBackButton.GetComponent<Button>().onClick.AddListener(GoBack);
    }

    private void OnDisable()
    {
        goBackButton.GetComponent<Button>().onClick.RemoveListener(GoBack);
    }

    private void SoundToggleValueChanged(bool value)
    {
        mixer.SetFloat("Master", value ? 0 : -80);
        PlayerPrefs.SetInt("Sound", value ? 1 : 0);
    }

    public void PrivacyPolicyButton()
    {
        Application.OpenURL("https://pages.flycricket.io/fair-kicker/privacy.html");
    }

    private void GoBack()
    {
        goBackButton.SetActive(false);
        ArdaTween.MoveTo(gameObject, ArdaTween.Hash(
            "targetPosition", startPosition,
            "duration", slideDuration,
            "setActive", false));
        // gameObject.SetActive(false);
    }
}