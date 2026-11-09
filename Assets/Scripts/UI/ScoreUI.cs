using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private MergeManager mergeManager;
    [SerializeField] private TMP_Text scoreText;

    private void OnEnable()
    {
        mergeManager.ScoreChanged+=OnScoreChanged;
    }

    private void OnDisable()
    {
        mergeManager.ScoreChanged -= OnScoreChanged;
    }

    private void Start()
    {
        OnScoreChanged(mergeManager.Score);
    }

    private void OnScoreChanged(int score)
    {
        scoreText.text=score.ToString();
    }
}
