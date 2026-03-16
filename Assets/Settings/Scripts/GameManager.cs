using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Orientation,
        TutorialGrab,
        TutorialPull,
        TutorialShoot,
        Ready,
        Playing,
        TimeOver
    }

    public GameState CurrentState { get; private set; }

    [Header("Gameplay References")]
    [SerializeField] private XRGrabInteractable bowGrab;
    [SerializeField] private XRGrabInteractable stringGrab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bool playTutorial = PlayerPrefs.GetInt("PlayTutorial", 0) == 1;

        if (playTutorial)
            SetState(GameState.Orientation);
        else
            SetState(GameState.Ready);
    }

    public void SetState(GameState newState)
    {
        Debug.Log("NEW STATE: " + newState);

        CurrentState = newState;

        switch (CurrentState)
        {
            case GameState.Orientation:
                DisableAllGameplay();
                break;

            case GameState.TutorialGrab:
                EnableBowAndString();
                break;

            case GameState.TutorialPull:
                Debug.Log("Enabling Bow + String");
                EnableBowAndString();
                break;

            case GameState.TutorialShoot:
                EnableBowAndString();
                break;

            case GameState.Ready:
                EnableBowOnly();
                break;

            case GameState.Playing:
                EnableBowAndString();
                SessionTracker.Instance.StartSession(PlayerManager.Instance.currentPlayer);
                RoundTimer.Instance.StartTimer();
                break;

            case GameState.TimeOver:
                EnableBowOnly();
                break;
        }


        TutorialController.Instance?.OnGameStateChanged(CurrentState);
    }

    private void DisableAllGameplay()
    {
        bowGrab.enabled = false;
        stringGrab.enabled = false;
    }

    private void EnableBowOnly()
    {
        bowGrab.enabled = true;
        stringGrab.enabled = false;
    }

    private void EnableBowAndString()
    {
        bowGrab.enabled = true;
        stringGrab.enabled = true;
    }

}
