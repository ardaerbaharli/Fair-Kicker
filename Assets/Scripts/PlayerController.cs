using System;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject ball;

    private Rigidbody2D ball_rb;
    private float _initGravityScale;
    private AudioSource kickSound;
    private AudioSource bounceSound;

    private void Awake()
    {
        kickSound = GetComponent<AudioSource>();
        bounceSound = ball.GetComponent<AudioSource>();
        ball_rb = ball.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _initGravityScale = ball_rb.gravityScale;
        GameManager.Instance.gameOver += GameOver;
    }

    private void GameOver()
    {
        Destroy(ball_rb.gameObject);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.Instance.gameOver -= GameOver;
    }

    public void KickTheBall(float kickingForce, float gravityMultiplier)
    {
        kickSound.Play();

        ball_rb.velocity = Vector2.zero;

        ball_rb.gravityScale = _initGravityScale * gravityMultiplier;
        var force = new Vector2(0, kickingForce);
        ball_rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void BallBouncedOffGround(float kickingForce, float gravityMultiplier)
    {
        bounceSound.Play();

        ball_rb.velocity = Vector2.zero;

        ball_rb.gravityScale = _initGravityScale * gravityMultiplier;
        var force = new Vector2(0, kickingForce);
        ball_rb.AddForce(force, ForceMode2D.Impulse);
    }
    public void StartBall()
    {
    }
}