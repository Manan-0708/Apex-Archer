using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;
    public SessionTracker SessionTracker => SessionTracker.Instance;

    private bool hasPulled = false;
    private bool hasShot = false;

    private void Awake()
    {
        Instance = this;
    }

    public void OnGameStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Orientation:
                string playerName = PlayerManager.Instance.currentPlayer;
                GameUIManager.Instance.ShowMessage(
                    "WELCOME " + playerName.ToUpper() + "\n\nLook at your controllers.\nPress Pinch to continue."
                );
                break;

            case GameManager.GameState.TutorialGrab:
                GameUIManager.Instance.ShowMessage(
                    "STEP 1\n\nGrab the bow with your LEFT hand.\nGrab string with your RIGHT Hand."
                );
                break;

            case GameManager.GameState.TutorialPull:
                GameUIManager.Instance.ShowMessage(
                    "STEP 2\n\nPull the string with your RIGHT hand."
                );
                break;

            case GameManager.GameState.TutorialShoot:
                GameUIManager.Instance.ShowMessage(
                    "STEP 3\n\nRelease to shoot."
                );
                break;

            case GameManager.GameState.Ready:
                GameUIManager.Instance.ShowMessage(
                    "Great!\n\nYou have 2 minutes.\nPress Pinch to begin."
                );
                break;

            case GameManager.GameState.Playing:
                GameUIManager.Instance.HidePopup();
                break;
        }
    }

    public void OnBowGrab()
    {
        if (GameManager.Instance.CurrentState ==
            GameManager.GameState.TutorialGrab)
        {
            GameManager.Instance.SetState(GameManager.GameState.TutorialPull);
        }
    }

    public void OnBowPulled(float strength)
    {
        if (!hasPulled &&
            GameManager.Instance.CurrentState ==
            GameManager.GameState.TutorialPull &&
            strength > 0.3f)
        {
            hasPulled = true;
            GameManager.Instance.SetState(GameManager.GameState.TutorialShoot);
        }
    }

    public void OnArrowShot()
    {
        if (!hasShot &&
            GameManager.Instance.CurrentState ==
            GameManager.GameState.TutorialShoot)
        {
            hasShot = true;
            GameManager.Instance.SetState(GameManager.GameState.Ready);
        }
    }
}
