using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VRRoundInput : MonoBehaviour
{
    [SerializeField] private InputActionReference rightPrimaryButton;

    private bool wasPressed = false;

    private void OnEnable()
    {
        if (rightPrimaryButton != null)
            rightPrimaryButton.action.Enable();
    }

    private void OnDisable()
    {
        if (rightPrimaryButton != null)
            rightPrimaryButton.action.Disable();
    }

    private void Update()
    {
        if (rightPrimaryButton == null) return;

        bool isPressed = rightPrimaryButton.action.ReadValue<float>() > 0.5f;

        if (isPressed && !wasPressed)
        {
            HandlePress();
        }

        wasPressed = isPressed;
    }

    private void HandlePress()
    {
        var state = GameManager.Instance.CurrentState;

        if (state == GameManager.GameState.Orientation)
        {
            GameManager.Instance.SetState(GameManager.GameState.TutorialGrab);
            return;
        }

        if (state == GameManager.GameState.Ready)
        {
            GameManager.Instance.SetState(GameManager.GameState.Playing);
            return;
        }

        if (state == GameManager.GameState.TimeOver)
        {
            SceneManager.LoadScene("Basic Main Menu");
        }
    }
}
