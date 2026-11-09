using UnityEngine;

public static class Haptics
{
    public static void Vibrate()
    {
        if(!GameSettings.VibrationOn) return;
    }
}
