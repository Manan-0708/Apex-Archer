using UnityEngine;

public class RadialMenuManager : MonoBehaviour
{
    public GameObject playerListPanel;
    public GameObject sessionDetailsPanel;
    public GameObject leaderboardPanel;
    public GameObject startPanel;   // your New/Existing screen

    public void ShowPlayerList()
    {
        playerListPanel.SetActive(true);
        sessionDetailsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }

    public void ShowSessionDetails()
    {
        playerListPanel.SetActive(false);
        sessionDetailsPanel.SetActive(true);
        leaderboardPanel.SetActive(false);
    }

    public void ShowLeaderboard()
    {
        playerListPanel.SetActive(false);
        sessionDetailsPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

    public void ExitToStart()
    {
        transform.parent.gameObject.SetActive(false); // hide RadialUI
        startPanel.SetActive(true);                  // show StartPanel
    }
}
