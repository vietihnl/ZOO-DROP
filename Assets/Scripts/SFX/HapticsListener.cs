using UnityEngine;

public class HapticsListener : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        gameManager.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        gameManager.GameOver -= OnGameOver;
    }

    private void OnGameOver(int score, int best)
    {
        Haptics.Vibrate();
    }
}
