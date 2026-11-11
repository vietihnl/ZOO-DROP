using UnityEngine;

public static class ScoreStorage
{
    private const string BestKey = "HighScore";
    public static int Best => PlayerPrefs.GetInt(BestKey,0);

    public static bool TrySetBest(int score)
    {
        if(score<=Best) return false;

        PlayerPrefs.SetInt(BestKey, score);
        PlayerPrefs.Save();
        return true;
    }
}

