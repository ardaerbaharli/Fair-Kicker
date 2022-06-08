using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuPanelController : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject betsPanel;

    [SerializeField] private float slideDuration = 1f;

    private Vector3 centerPos;

    [SerializeField] private AudioMixer mixer;

    private void Start()
    {
        moneyText.text = PlayerPrefs.GetInt("Money", 0).ToString();
        centerPos = transform.position;
        
        var isSoundOn = PlayerPrefs.GetInt("Sound", 1) == 1;
        mixer.SetFloat("Master", isSoundOn ? 0 : -80);
    }

    public void PlayButton()
    {
        betsPanel.SetActive(true);
        ArdaTween.MoveTo(betsPanel, ArdaTween.Hash(
            "targetPosition", centerPos, "duration", slideDuration));
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true);
        ArdaTween.MoveTo(settingsPanel, ArdaTween.Hash(
            "targetPosition", centerPos, "duration", slideDuration));
    }
}