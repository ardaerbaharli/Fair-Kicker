using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int betAmount;
    [SerializeField] public float odd;
    [SerializeField] private int expectedKicksMin;
    [SerializeField] private int expectedKicksMax;
    [SerializeField] public int numberOfKicks;
    [SerializeField] public float remainingTime = 40;
    [SerializeField] public GameOverReason gameOverReason;
    [SerializeField] public float probabilityOfHitting = 20;
    [SerializeField] public float remainingSpeedTime;
    [SerializeField] public float kickingForce = 10f;
    [SerializeField] public float gravityMultiplier = 1;

    [SerializeField] public int moneyWon, moneyLost;

    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;

    private bool _isGameCompleted;
    public bool isGameCompleted;
    [SerializeField] private bool _isGameStarted;

    public bool IsGameStarted
    {
        get => _isGameStarted;
        set
        {
            _isGameStarted = value;
            if (_isGameStarted)
                StartGame();
        }
    }


    private PlayerController _pc;

    public enum GameOverReason
    {
        Win,
        Lose,
    }


    public enum Improvement
    {
        Time,
        Accurate,
        Speed
    }

    private readonly Dictionary<Improvement, int> improvementPrices = new Dictionary<Improvement, int>()
    {
        {Improvement.Time, 500},
        {Improvement.Accurate, 500},
        {Improvement.Speed, 500}
    };

    [SerializeField] private Dictionary<Improvement, int> ownedImprovements = new Dictionary<Improvement, int>();
    public static GameManager Instance;

    public delegate void GameOver();


    public event GameOver gameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetInt("FirstPlay", 1) == 1)
        {
            PlayerPrefs.SetInt("FirstPlay", 0);
            PlayerPrefs.SetInt("Money", 500);
            PlayerPrefs.SetString("LastMoneyDay", DateTime.Now.ToString(CultureInfo.CurrentCulture));
        }

        var d = DateTime.Parse(PlayerPrefs.GetString("LastMoneyDay",
            DateTime.Now.ToString(CultureInfo.CurrentCulture)));
        if (d.AddDays(1) <= DateTime.Now)
        {
            var m = PlayerPrefs.GetInt("Money", 0);
            m += 500;
            PlayerPrefs.SetInt("Money", m);
            PlayerPrefs.SetString("LastMoneyDay", DateTime.Now.ToString(CultureInfo.CurrentCulture));
        }

        UpdateOwnedImprovements();
    }

    private void ResetValues()
    {
        numberOfKicks = 0;
        remainingTime = 40;
        probabilityOfHitting = 35;
        remainingSpeedTime = 0;
        gravityMultiplier = 1;
        IsGameStarted = false;
        isGameCompleted = false;
    }

    private void UpdateOwnedImprovements()
    {
        ownedImprovements.Clear();
        foreach (var i in Enum.GetValues(typeof(Improvement)))
        {
            var piece = PlayerPrefs.GetInt(i.ToString(), 0);
            ownedImprovements.Add((Improvement) i, piece);
        }
    }

    private void Update()
    {
        if (!IsGameStarted || isGameCompleted) return;
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
            StartCoroutine(GameCompleted());

        if (remainingSpeedTime > 0)
        {
            remainingSpeedTime -= Time.deltaTime;
        }
        else if (remainingSpeedTime <= 0)
        {
            SpeedOver();
        }
    }

    private bool didHitGround;

    public void ShootTrigger()
    {
        if (isGameCompleted) return;
        if (didHitGround)
        {
            didHitGround = false;
            return;
        }

        var threshold = Random.Range(1, 101);
        if (threshold <= probabilityOfHitting)
        {
            numberOfKicks++;
            _pc.KickTheBall(kickingForce * gravityMultiplier, gravityMultiplier);
            // if (numberOfKicks == 12)
            //     StartCoroutine(GameCompleted());
        }
    }

    public void GroundTrigger()
    {
        if (isGameCompleted) return;
        didHitGround = true;
        _pc.BallBouncedOffGround(kickingForce * gravityMultiplier, gravityMultiplier);
    }

    private IEnumerator GameCompleted()
    {
        isGameCompleted = true;
        yield return new WaitForSeconds(1.5f);
        if (numberOfKicks >= expectedKicksMin && numberOfKicks <= expectedKicksMax) Win();
        else Lose();
    }

    public void SetBet(int betAmount, float odd, int expectedKicksMin, int expectedKicksMax)
    {
        this.betAmount = betAmount;
        this.odd = odd;
        this.expectedKicksMin = expectedKicksMin;
        this.expectedKicksMax = expectedKicksMax;
        // var money = PlayerPrefs.GetInt("Money");
        // PlayerPrefs.SetInt("Money", money - this.betAmount);
    }

    public bool BuyImprovement(Improvement i)
    {
        var money = PlayerPrefs.GetInt("Money");
        if (money < improvementPrices[i]) return false;
        if (i == Improvement.Speed && ownedImprovements[Improvement.Speed] != 0) return false;

        PlayerPrefs.SetInt("Money", money - improvementPrices[i]);
        ownedImprovements[i]++;

        PlayerPrefs.SetInt(i.ToString(), ownedImprovements[i]);
        return true;
    }

    public bool SellImprovement(Improvement i)
    {
        var money = PlayerPrefs.GetInt("Money");
        if (money < improvementPrices[i]) return false;

        PlayerPrefs.SetInt("Money", money + improvementPrices[i]);
        ownedImprovements[i]--;
        PlayerPrefs.SetInt(i.ToString(), ownedImprovements[i]);

        return true;
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
        ResetValues();
    }

    private void StartGame()
    {
        _pc = FindObjectOfType<PlayerController>();
        _pc.StartBall();
    }

    public void Win()
    {
        winSound.Play();
        _isGameCompleted = true;
        gameOverReason = GameOverReason.Win;
        moneyWon = (int) (betAmount * odd);

        var money = PlayerPrefs.GetInt("Money");
        PlayerPrefs.SetInt("Money", money + moneyWon);
        gameOver?.Invoke();
    }

    public void Lose()
    {
        loseSound.Play();
        _isGameCompleted = true;
        gameOverReason = GameOverReason.Lose;
        moneyLost = betAmount;

        var money = PlayerPrefs.GetInt("Money");
        PlayerPrefs.SetInt("Money", money - moneyLost);

        gameOver?.Invoke();
    }

    public bool UseImprovement(Improvement i)
    {
        if (ownedImprovements[i] == 0) return false;
        ownedImprovements[i]--;
        PlayerPrefs.SetInt(i.ToString(), ownedImprovements[i]);

        switch (i)
        {
            case Improvement.Time:
                PlusTime();
                break;
            case Improvement.Accurate:
                PlusAccurate();
                break;
            case Improvement.Speed:
                PlusSpeed();
                break;
        }

        return true;
    }

    public void PlusAccurate()
    {
        var a = 5;
        probabilityOfHitting += a;
    }

    private float plusSpeedBonus;

    public void PlusSpeed()
    {
        remainingSpeedTime += 10;
        gravityMultiplier = 2;

        plusSpeedBonus = probabilityOfHitting * 0.05f;
        probabilityOfHitting += plusSpeedBonus;
    }

    public void SpeedOver()
    {
        gravityMultiplier = 1;
        probabilityOfHitting -= plusSpeedBonus;
    }

    public void PlusTime()
    {
        remainingTime += 10;
    }
}