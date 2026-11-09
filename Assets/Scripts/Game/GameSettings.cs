using UnityEngine;

public static class GameSettings
{
    private const string MusicKey = "Setting_Music";
    private const string SfxKey = "Setting_Sfx";
    private const string VibrationKey = "Setting_Vibration";

    private static bool loaded;
    private static bool musicOn = true;
    private static bool sfxOn = true;
    private static bool vibrationOn = true;

    public static event System.Action Changed;

    public static bool MusicOn {get{Load(); return musicOn;}}
    public static bool SfxOn { get{Load(); return sfxOn;}}
    public static bool VibrationOn { get{Load(); return vibrationOn;}}

    public static void SetMusic(bool on)
    {
        Load();
        if(musicOn == on) return;
        musicOn = on;
        Save(MusicKey,on);
    }
    
    public static void SetSfx(bool on)
    {
        Load();
        if(sfxOn == on) return;
        sfxOn = on;
        Save(SfxKey,on);
    }

    public static void SetVibration(bool on)
    {
        Load();
        if(vibrationOn == on) return;
        vibrationOn = on;
        Save(VibrationKey, on);
    }

    public static void Load()
    {
        if(loaded) return;
        loaded= true;

        musicOn = PlayerPrefs.GetInt(MusicKey, 1) == 1;
        sfxOn = PlayerPrefs.GetInt(SfxKey, 1) == 1;
        vibrationOn = PlayerPrefs.GetInt(VibrationKey, 1) == 1;
    }

    public static void Save(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
        Changed?.Invoke();
    }

}