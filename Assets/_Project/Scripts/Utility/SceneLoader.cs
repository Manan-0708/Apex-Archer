using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void LoadGame(bool tutorial)
    {
        PlayerPrefs.SetInt("PlayTutorial", tutorial ? 1 : 0);
        SceneManager.LoadScene("Desert_Scene");
    }
}