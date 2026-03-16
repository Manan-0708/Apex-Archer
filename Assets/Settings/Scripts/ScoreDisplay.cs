using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;

    private void Update()
    {
        if (SessionTracker.Instance == null || PlayerManager.Instance == null || RoundTimer.Instance == null)
            return;

        string timerText;

        // Before round starts
        if (!RoundTimer.Instance.IsTimeOver && RoundTimer.Instance.TimeRemaining >= 120f)
            timerText = "PRESS RIGHT BUTTON TO START";

        // After time is over
        else if (RoundTimer.Instance.IsTimeOver)
            timerText = "TIME OVER — PRESS RIGHT BUTTON TO RESTART";

        // During the round
        else
            timerText = "TIME: " + RoundTimer.Instance.GetFormattedTime();

        scoreText.text =
            timerText + "\n\n" +
            "PLAYER: " + PlayerManager.Instance.currentPlayer + "\n" +
            "ARROWS: " + SessionTracker.Instance.ArrowsShot + "\n" +
            "HITS: " + SessionTracker.Instance.Hits + "\n" +
            "MISSES: " + SessionTracker.Instance.Misses + "\n" +
            "ACCURACY: " + (SessionTracker.Instance.Accuracy * 100f).ToString("F1") + "%\n" +
            "SCORE: " + SessionTracker.Instance.Score.ToString("F0");
    }

}
