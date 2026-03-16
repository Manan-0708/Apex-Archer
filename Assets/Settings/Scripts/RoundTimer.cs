using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundTimer : MonoBehaviour
{
    public static RoundTimer Instance;

    [SerializeField] private float roundDuration = 60f;

    private float timeRemaining;
    private bool isRunning = false;

    public bool IsRunning => isRunning;
    public float TimeRemaining => timeRemaining;
    public bool IsTimeOver => timeRemaining <= 0f;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTimer()
    {
        timeRemaining = roundDuration;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;

            SessionTracker.Instance.EndSession();
            GameManager.Instance.SetState(GameManager.GameState.TimeOver);
            GameUIManager.Instance.ShowGameOver(
                (int)SessionTracker.Instance.Score
            );
        }
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void RestartRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
