using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelController : MonoBehaviour
{
    [SerializeField] private float slideDuration = 0.5f;

    [SerializeField] private Text timeText;
    [SerializeField] private Text numberOfKicksText;
    [SerializeField] private GameObject gameOverPanel;

    private Vector3 centerPos;

    private void Awake()
    {
        GameManager.Instance.IsGameStarted = true;
        GameManager.Instance.gameOver += GameOver;
        centerPos = transform.position;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted || GameManager.Instance.isGameCompleted) return;

        timeText.text = $"00:{Mathf.RoundToInt(GameManager.Instance.remainingTime)}";
        numberOfKicksText.text = GameManager.Instance.numberOfKicks.ToString();
    }

    private void OnDestroy()
    {
        GameManager.Instance.gameOver -= GameOver;
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        ArdaTween.MoveTo(gameOverPanel, ArdaTween.Hash(
            "targetPosition", centerPos, "duration", slideDuration));
    }

    public void UseTimeButton()
    {
        var result = GameManager.Instance.UseImprovement(GameManager.Improvement.Time);
        if (result)
        {
        }
    }

    public void UseAccurateButton()
    {
        var result = GameManager.Instance.UseImprovement(GameManager.Improvement.Accurate);
        if (result)
        {
        }
    }

    public void UseSpeedButton()
    {
        var result = GameManager.Instance.UseImprovement(GameManager.Improvement.Speed);
        if (result)
        {
        }
    }
}