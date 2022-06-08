using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameOverPanelController : MonoBehaviour
    {
        [SerializeField] private float slideDuration = 0.5f;

        [SerializeField] private Text winLoseText;
        [SerializeField] private Text numberOfKicksText;
        [SerializeField] private Text money;
        
        private Vector3 startPosition;


        private void Awake()
        {
            startPosition = transform.position;
            SetWinLoseText();
            SetNumberOfKicksText();
            money.text = PlayerPrefs.GetInt("Money").ToString();
        }

        private void SetNumberOfKicksText()
        {
         numberOfKicksText.text = $"Kicks: {GameManager.Instance.numberOfKicks}";   
        }
        private void SetWinLoseText()
        {
            var r = GameManager.Instance.gameOverReason;

            winLoseText.text = r == GameManager.GameOverReason.Win
                ? $"You win: {GameManager.Instance.moneyWon}"
                : $"You lose: {GameManager.Instance.moneyLost}";
        }

        public void MenuButton()
        {
            SceneManager.LoadScene("Main");
        }

        public void AgainButton()
        {
            MenuButton();
            /*
            betsPanel.SetActive(true);
            ArdaTween.MoveTo(betsPanel, ArdaTween.Hash(
                "targetPosition", startPosition, "duration", slideDuration, "onComplete", "Hide"));
*/
        }

        public void Hide()
        {
            ArdaTween.MoveTo(gameObject, ArdaTween.Hash(
                "targetPosition", startPosition, "duration", slideDuration, "setActive", false));
        }
    }
}