using UnityEngine;
using TMPro;

public class BestScoreUI : MonoBehaviour
{
    [SerializeField] private MergeManager mergeManager;
    [SerializeField] private TMP_Text bestText;
    [SerializeField] private string preflix= "BEST";

    private void OnEnable()
    {
        if(mergeManager != null) mergeManager.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        if(mergeManager != null) mergeManager.ScoreChanged -= OnScoreChanged;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnScoreChanged(int score)
    {
        Refresh();
    }

    private void Refresh()
    {
        bestText.text = preflix +ScoreStorage.Best;
    }
}
