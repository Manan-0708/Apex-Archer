using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class KeyboardHaptics : MonoBehaviour
{
    [SerializeField] private HapticSender hapticSender_Right;
    [SerializeField] private HapticSender hapticSender_Left;

    [SerializeField] private float amplitude = 0.15f;
    [SerializeField] private float duration = 0.03f;

    private void Start()
    {
        if (NonNativeKeyboard.Instance != null)
        {
            NonNativeKeyboard.Instance.OnKeyboardValueKeyPressed += OnKeyPressed;
        }
    }

    private void OnKeyPressed(KeyboardValueKey key)
    {
        if (hapticSender_Right != null && hapticSender_Left != null)
        {
            hapticSender_Left.SendHapticImpulse(amplitude, duration);
            hapticSender_Right.SendHapticImpulse(amplitude, duration);
        }
    }
}