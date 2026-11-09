using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private MergeManager mergeManager;
    [SerializeField] private GameManager gameManager;

    [Header("Sound")]
    [SerializeField] private AudioClip mergeClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;
    [SerializeField] private float pitchPerLevel = 0.05f;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField, Range(0f,1f)] private float musicVolumeOnGameOver=0.15f;
    
    [Header("Hit")]
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioClip hitClip;
    [SerializeField, Range(0f, 1f)] private float hitVolume = 0.6f;
    [SerializeField] private float hitMaxSpeed=12f;
    [SerializeField] private float hitMinInterval = 0.05f;
    [SerializeField] private float hitPitchPerLevel = -0.04f;

    private float lastHitTime;


    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
       
    }

    private void OnEnable()
    {
        
        mergeManager.Merged += OnMerged;
        gameManager.GameOver += OnGameOver;
        Animal.Hit += OnAnimalHit;
    }

    private void OnDisable()
    {

        mergeManager.Merged -= OnMerged;
        gameManager.GameOver -= OnGameOver;
        Animal.Hit -= OnAnimalHit;
    }


    private void OnMerged(int level, Vector3 position)
    {
        Play(mergeClip,1f + level*pitchPerLevel);
    }

    private void OnGameOver(int score, int best)
    {
        Play(gameOverClip, 1f);
    }

    private void Play(AudioClip clip, float pitch)
    {          
        if(clip == null|| !GameSettings.SfxOn) return;

        source.pitch = pitch;
        source.PlayOneShot(clip,volume);
    }

    private void OnAnimalHit(int level,float speed, Vector3 position)
    {
        if(hitClip == null||hitSource == null||!GameSettings.SfxOn) return;

        if(Time.time - lastHitTime < hitMinInterval) return;
        lastHitTime = Time.time;

        float t = Mathf.Clamp01(speed/hitMaxSpeed);
        float loudness = Mathf.Lerp(0.2f, 1f, t);

        hitSource.pitch = Mathf.Max(0.5f, 1f+level*hitPitchPerLevel);
        hitSource.PlayOneShot(hitClip,hitVolume*loudness);
    }
}
