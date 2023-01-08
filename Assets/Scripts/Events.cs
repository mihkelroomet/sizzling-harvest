using System;

public class Events
{
    public static event Action<float> OnSetTime;
    public static void SetTime(float value) => OnSetTime?.Invoke(value);
    public static event Func<float> OnGetTime;
    public static float GetTime() => OnGetTime?.Invoke() ?? 0;

    public static event Action<int> OnSetScore;
    public static void SetScore(int value) => OnSetScore?.Invoke(value);
    public static event Func<int> OnGetScore;
    public static int GetScore() => OnGetScore?.Invoke() ?? 0;

    
    public static event Action OnStartGame;
    public static void StartGame() => OnStartGame?.Invoke();
    public static event Action OnEndGame;
    public static void EndGame() => OnEndGame?.Invoke();
}
