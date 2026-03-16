using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public enum ButtonType { Start, Quit }
    public ButtonType buttonType;

    [SerializeField] private GameObject menuBoard;

    public void OnPressed()
    {
        Debug.Log("Pressed: " + buttonType);

        switch (buttonType)
        {
            case ButtonType.Start:
                if (string.IsNullOrEmpty(PlayerManager.Instance.currentPlayer))
                {
                    menuBoard.SetActive(false);
                    PlayerInputManager.Instance.ShowInputBoard();
                }
                else
                {
                    SceneLoader.Instance.LoadGame(false);
                }
                break;

            case ButtonType.Quit:
                Debug.Log("Quitting game...");
                Application.Quit();
                break;
        }
    }
}