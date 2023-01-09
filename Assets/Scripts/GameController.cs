using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private bool _seenIntroDialogue;
    private bool _firstSpeedIncreaseDone;
    private bool _secondSpeedIncreaseDone;

    // Dialogue Panels
    public float ShowDialoguePanelAt;
    public GameObject IntroDialoguePanel;
    public GameObject WinDialoguePanel;
    public GameObject LossDialoguePanel;

    public Canvas HUD;
    public Texture2D CursorDefault;
    public Vector2 CursorDefaultHotspot;

    public int Score {get; set;}
    public int RequiredScore;
    public float TimeLeft {get; set;}
    public int RoundTime;
    public bool GameRunning {get; set;}
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _seenIntroDialogue = false;
        IntroDialoguePanel.SetActive(false);
        WinDialoguePanel.SetActive(false);
        LossDialoguePanel.SetActive(false);
        HUD.gameObject.SetActive(false);
        Cursor.SetCursor(CursorDefault, CursorDefaultHotspot, CursorMode.ForceSoftware);

        Events.OnStartGame += StartGame;
        Events.OnEndGame += EndGame;
        Events.OnGetTime += GetTime;
        Events.OnSetTime += SetTime;
        Events.OnGetScore += GetScore;
        Events.OnSetScore += SetScore;

        GameRunning = false;
    }

    void Update()
    {
        if (!_seenIntroDialogue && Time.time >= ShowDialoguePanelAt)
        {
            IntroDialoguePanel.SetActive(true);
            _seenIntroDialogue = true;
        }

        if (GameRunning)
        {
            Events.SetTime(Events.GetTime() - Time.deltaTime);

            if (Events.GetTime() <= 0)
            {
                Events.EndGame();
            }

            if (!_firstSpeedIncreaseDone && TimeLeft < RoundTime * 2 / 3)
            {
                _firstSpeedIncreaseDone = true;
                IncreaseFruitSpawnFrequency();
            }

            if (!_secondSpeedIncreaseDone && TimeLeft < RoundTime / 3)
            {
                _secondSpeedIncreaseDone = true;
                IncreaseFruitSpawnFrequency();
            }
        }
    }

    void IncreaseFruitSpawnFrequency()
    {
        FruitSpawner.Instance.FrameSpawnProbability *= 2.5f;
        FruitSpawner.Instance.MinDelay *= 0.7f;
    }

    void StartGame()
    {
        _firstSpeedIncreaseDone = false;
        _secondSpeedIncreaseDone = false;
        FruitSpawner.Instance.Active = true;
        FruitSpawner.Instance.ResetParameters();
        HUD.gameObject.SetActive(true);
        GameRunning = true;
        Events.SetScore(0);
        Events.SetTime(RoundTime);
    }

    void EndGame()
    {
        FruitSpawner.Instance.Active = false;
        HUD.gameObject.SetActive(false);
        GameRunning = false;

        if (Score >= RequiredScore)
        {
            WinDialoguePanel.SetActive(true);
        }
        else
        {
            LossDialoguePanel.SetActive(true);
        }
    }

    float GetTime()
    {
        return TimeLeft;
    }

    void SetTime(float value)
    {
        TimeLeft = value;
    }

    int GetScore()
    {
        return Score;
    }

    void SetScore(int value)
    {
        Score = value;
    }

    private void OnDestroy()
    {
        Events.OnStartGame -= StartGame;
        Events.OnEndGame -= EndGame;
        Events.OnGetTime -= GetTime;
        Events.OnSetTime -= SetTime;
        Events.OnGetScore -= GetScore;
        Events.OnSetScore -= SetScore;
    }
}
