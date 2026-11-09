using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    [SerializeField] private MergeManager mergeManager;
    [SerializeField] private string mainMenuSceneName = "MainScene";

    private bool isPaused;

    private bool isGameOver;

    public event System.Action<int, int> GameOver;

    public bool IsGameOver => isGameOver;

    public bool IsPaused => isPaused;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    public void TriggerGameOver()
    {
        if(isGameOver) return;
        isGameOver = true;

        int score = mergeManager.Score;
        int best = PlayerPrefs.GetInt(HighScoreKey, 0);
        if(score>best)
        {
            best = score;
            PlayerPrefs.SetInt(HighScoreKey, best);
            PlayerPrefs.Save();
        }

        Time.timeScale = 0f;
        GameOver?.Invoke(score, best);
    }

    public void Restart()
    {
        Time.timeScale=1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetPaused(bool paused){
        if(isGameOver) return;
        isPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
