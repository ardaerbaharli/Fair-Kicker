using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.GroundTrigger();
        }
    }
}
