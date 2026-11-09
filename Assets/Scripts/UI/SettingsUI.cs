using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject panel;

    [Header("Buttons")]
    [SerializeField] private Button openButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    [Header("switch")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle vibrationToggle;

    private void OnEnable()
    {
        openButton.onClick.AddListener(Open);
        resumeButton.onClick.AddListener(Close);
        menuButton.onClick.AddListener(OnMenuCliked);

        musicToggle.onValueChanged.AddListener(GameSettings.SetMusic);
        sfxToggle.onValueChanged.AddListener(GameSettings.SetSfx);
        vibrationToggle.onValueChanged.AddListener(OnVibrationChanged);
    }

    private void OnDisable()
    {
        openButton.onClick.RemoveListener(Open);
        resumeButton.onClick.RemoveListener(Close);
        menuButton.onClick.RemoveListener(OnMenuCliked);

        musicToggle.onValueChanged.RemoveListener(GameSettings.SetMusic);
        sfxToggle.onValueChanged.RemoveListener(GameSettings.SetSfx);
        vibrationToggle.onValueChanged.RemoveListener(OnVibrationChanged);
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    private void Open()
    {
        if(gameManager.IsGameOver) return;

        musicToggle.SetIsOnWithoutNotify(GameSettings.MusicOn);
        sfxToggle.SetIsOnWithoutNotify(GameSettings.SfxOn);
        vibrationToggle.SetIsOnWithoutNotify(GameSettings.VibrationOn);

        panel.SetActive(true);
        gameManager.SetPaused(true);
    }

    private void Close()
    {
        panel.SetActive(false);
        gameManager.SetPaused(false);
    }

    private void OnMenuCliked()
    {
        gameManager.GoToMainMenu();
    }

    private void OnVibrationChanged(bool on)
    {
        GameSettings.SetVibration(on);
        if(on) Haptics.Vibrate();
    }
}
