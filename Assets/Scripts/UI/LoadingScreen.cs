using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "MainScene";
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private float minShowTime = 2f;
    [SerializeField] private float dotInterval = 0.4f;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private IEnumerator Start()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        float elapsed = 0f;
        float dotTimer = 0f;
        int dots = 0;

        loadingText.text = "Loading";
        progressBar.value = 0f;

        while(!op.isDone)
        {
            elapsed += Time.unscaledDeltaTime;

            float loadProgress = Mathf.Clamp01(op.progress/0.9f);
            float timeProgress = Mathf.Clamp01(elapsed/minShowTime);
            float shown = Mathf.Min(loadProgress, timeProgress);
            progressBar.value = shown;

            dotTimer += Time.unscaledDeltaTime;
            if(dotTimer >= dotInterval)
            {
                dotTimer = 0f;
                dots = (dots + 1)%4;
                loadingText.text = "Loading" + new string('.', dots);
            }

            if(shown >= 1f)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
