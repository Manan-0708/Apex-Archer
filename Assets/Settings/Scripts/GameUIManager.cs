using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;


    [Header("3D Board References")]
    public GameObject tutorialBoard;   // Parent object (wood + parchment)
    public TMP_Text tutorialText;      // 3D TextMeshPro

    private void Awake()
    {
        Instance = this;
    }

    public void ShowMessage(string message)
    {
        tutorialBoard.SetActive(true);
        tutorialText.text = message;
    }

    public void HidePopup()
    {
        tutorialBoard.SetActive(false);
    }

    public void ShowGameOver(int finalScore)
    {
        tutorialBoard.SetActive(true);
        tutorialText.text =
            "TIME OVER\n\n" +
            "FINAL SCORE: " + finalScore + "\n\n" +
            "PRESS Pinch TO GO TO MAIN MENU";

    }
}
