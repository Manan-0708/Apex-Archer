using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public string currentPlayer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SetPlayer(string name)
    {
        currentPlayer = name;
        Debug.Log("Current Player Set: " + name);
        PlayerManager.Instance.currentPlayer = name;
    }

    //public void RegisterNewPlayer(string name)
    //{
    //    currentPlayer = name;
    //    FileDataManager.Instance.AddPlayer(name);
    //    Debug.Log("Current Player Set: " + name);
    //}

    //public void SetExistingPlayer(string name)
    //{
    //    currentPlayer = name;
    //    Debug.Log("Existing Player Selected: " + name);
    //}
}
