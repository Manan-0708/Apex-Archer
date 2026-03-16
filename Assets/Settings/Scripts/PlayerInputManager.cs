using TMPro;
using UnityEngine;
using UnityEngine.tvOS;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    [SerializeField] private GameObject inputBoard;
    [SerializeField] private GameObject tutorialBoard;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Hide input board initially
        if (inputBoard != null)
            inputBoard.SetActive(false);

        if (tutorialBoard != null)
            tutorialBoard.SetActive(false);
    }

    public void ShowInputBoard()
    {
        // Hide old keyboard board
        if (inputBoard != null)
            inputBoard.SetActive(false);

        // Start voice input instead
        VoiceInputManager.Instance.StartVoiceInput();
    }

    public void ShowTutorialBoard()
    {
        tutorialBoard.SetActive(true);
    }

    public void TutorialYes()
    {
        PlayerPrefs.SetInt("PlayTutorial", 1);
        SceneLoader.Instance.LoadGame(true);
    }

    public void TutorialNo()
    {
        PlayerPrefs.SetInt("PlayTutorial", 0);
        SceneLoader.Instance.LoadGame(false);
    }
}