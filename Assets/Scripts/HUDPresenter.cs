using UnityEngine;
using TMPro;

public class HUDPresenter : MonoBehaviour
{
    public static HUDPresenter Instance;
    public TMP_Text TimeCounterText;
    public TMP_Text ScoreCounterText;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        Events.OnSetTime += SetTime;
        Events.OnSetScore += SetScore;
    }

    void SetTime(float value)
    {
        TimeCounterText.text = Mathf.Round(value + 0.49f).ToString();
    }

    void SetScore(int value)
    {
        ScoreCounterText.text = value.ToString() + "/" + GameController.Instance.RequiredScore.ToString();
    }

    private void OnDestroy()
    {
        Events.OnSetTime -= SetTime;
        Events.OnSetScore -= SetScore;
    }
}
