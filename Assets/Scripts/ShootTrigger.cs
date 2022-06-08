using UnityEngine;

public class ShootTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.ShootTrigger();
        }
    }
}