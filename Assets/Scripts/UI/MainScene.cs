using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField] private string gameSceneName="GameScene";
    [SerializeField] private Button playButton;

    private bool isLoading;

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        if(isLoading) return;
        isLoading = true;

        SceneManager.LoadScene(gameSceneName);
    }
}
