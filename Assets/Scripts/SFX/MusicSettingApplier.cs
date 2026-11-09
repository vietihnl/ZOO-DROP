using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSettingApplier : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameSettings.Changed += Apply;
        Apply();
    }

    private void OnDisable()
    {
        GameSettings.Changed -= Apply;
    }

    private void Apply()
    {
        source.mute = !GameSettings.MusicOn;
    }
}
