using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestText;
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        gameManager.GameOver += OnGameOver;
        restartButton.onClick.AddListener(gameManager.Restart);
    }

    private void OnDisable()
    {
        gameManager.GameOver -= OnGameOver;
        restartButton.onClick.RemoveListener(gameManager.Restart);
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    private void OnGameOver(int score, int best)
    {
        scoreText.text = "SCORE: "+score;
        bestText.text = "BEST: "+best;
        panel.SetActive(true);
    }
}
