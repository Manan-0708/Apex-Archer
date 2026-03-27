using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject nameRegistrationPanel;
    [SerializeField] private GameObject mapSelectionPanel;
    [SerializeField] private GameObject gameModePanel;
    [SerializeField] private GameObject leaderboardPanel;

    [Header("References")]
    [SerializeField] private TMP_Text nameDisplayText;
    [SerializeField] private TMP_Text leaderboardText;
    [SerializeField] private Transform mapButtonContainer;
    [SerializeField] private GameObject mapButtonPrefab;
    [SerializeField] private GameObject nextButton;  // drag NextButton here in Inspector

    private void Start()
    {
        if (NarratorSystem.Instance != null)
            NarratorSystem.Instance.PlayWelcome();
            
        ShowPanel(mainMenuPanel);
    }

    // Called by START button
    public void OnStartPressed()
    {
        if (NarratorSystem.Instance != null)
            NarratorSystem.Instance.PlaySpeakName();
            
        if (voiceSystem != null)
        {
            voiceSystem.currentMode = VoiceCommandSystem.VoiceMode.NameCapture;
            voiceSystem.StartListening();
        }
            
        ShowPanel(nameRegistrationPanel);
    }

    // Called after name is captured
    public void OnNameConfirmed(string playerName)
    {
        if (voiceSystem != null)
            voiceSystem.StopListening();

        PlayerManager.Instance.SetPlayer(playerName);
        nameDisplayText.text = playerName;       // show the captured name
        nextButton.SetActive(true);              // ← NOW show the NEXT button
    }

    // Called by NEXT button after name entry
    public void OnNextPressed()
    {
        if (voiceSystem != null)
        {
            voiceSystem.currentMode = VoiceCommandSystem.VoiceMode.Commands;
            voiceSystem.StartListening();
        }
            
        if (NarratorSystem.Instance != null)
            NarratorSystem.Instance.PlaySelectMap();
            
        ShowPanel(mapSelectionPanel);
    }

    // Called when a map button is pressed
    public void OnMapSelected(string sceneName)
    {
        if (voiceSystem != null)
            voiceSystem.StopListening();

        SceneLoader.Instance.SelectMap(sceneName);
        
        if (NarratorSystem.Instance != null)
            NarratorSystem.Instance.PlayChooseMode();
            
        ShowPanel(gameModePanel);
    }

    // Called by PLAY TUTORIAL button
    public void OnPlayTutorial() => SceneLoader.Instance.LoadGame(true);

    // Called by PLAY GAME button
    public void OnPlayGame() => SceneLoader.Instance.LoadGame(false);

    // Called by LEADERBOARD button
    public void OnLeaderboardPressed()
    {
        PopulateLeaderboard();
        ShowPanel(leaderboardPanel);
    }

    // Called by QUIT button
    public void OnQuitPressed() => Application.Quit();

    // Called by leaderboard CLOSE button
    public void OnLeaderboardClose() => ShowPanel(mainMenuPanel);

    // --- Helpers ---
    private void ShowPanel(GameObject panel)
    {
        mainMenuPanel.SetActive(panel == mainMenuPanel);
        nameRegistrationPanel.SetActive(panel == nameRegistrationPanel);
        mapSelectionPanel.SetActive(panel == mapSelectionPanel);
        gameModePanel.SetActive(panel == gameModePanel);
        leaderboardPanel.SetActive(panel == leaderboardPanel);
    }

    private void PopulateMapButtons() { /* Dynamically spawn buttons from SceneLoader.GetAvailableMaps() */ }
    // This reads from your EXISTING FileDataManager.cs
    // In MainMenuController
    private void PopulateLeaderboard()
    {
        var entries = FileDataManager.Instance.GetTopEntries(10);
        var sb = new System.Text.StringBuilder();

        for (int i = 0; i < entries.Count; i++)
            sb.AppendLine($"{i + 1}. {entries[i].player} – {entries[i].score}");

        if (entries.Count == 0)
            sb.AppendLine("No scores yet!");

        leaderboardText.text = sb.ToString();
    }



    [SerializeField] private VoiceCommandSystem voiceSystem;

    private void OnEnable()
    {
        if (voiceSystem != null)
        {
            voiceSystem.OnCommandRecognized += HandleVoiceCommand;
            voiceSystem.OnNameCaptured += OnNameConfirmed;
        }
    }

    private void HandleVoiceCommand(string command)
    {
        switch (command)
        {
            case "start": OnStartPressed(); break;
            case "leaderboard": OnLeaderboardPressed(); break;
            case "quit": OnQuitPressed(); break;
            case "next": OnNextPressed(); break;
            case "jungle": OnMapSelected("Jungle_Scene"); break;
            case "desert": OnMapSelected("Desert_Scene"); break;
            case "stadium": OnMapSelected("Stadium_Scene"); break;
            case "tutorial": OnPlayTutorial(); break;
            case "playgame": OnPlayGame(); break;
        }
    }

}

