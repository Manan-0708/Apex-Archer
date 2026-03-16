using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRBaseInteractable))]
public class UIButtonFeedback : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.cyan;
    [SerializeField] private Color pressedColor = Color.green;

    [Header("Haptics")]
    [SerializeField] private float hoverAmplitude = 0.1f;
    [SerializeField] private float hoverDuration = 0.05f;
    [SerializeField] private float pressAmplitude = 0.3f;
    [SerializeField] private float pressDuration = 0.1f;

    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();

        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        targetRenderer.material.color = normalColor;
    }

    private void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
        interactable.selectEntered.AddListener(OnSelectEnter);
        interactable.selectExited.AddListener(OnSelectExit);
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);
        interactable.selectEntered.RemoveListener(OnSelectEnter);
        interactable.selectExited.RemoveListener(OnSelectExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        targetRenderer.material.color = hoverColor;
        SendHaptics(args.interactorObject, hoverAmplitude, hoverDuration);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        targetRenderer.material.color = normalColor;
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        targetRenderer.material.color = pressedColor;
        SendHaptics(args.interactorObject, pressAmplitude, pressDuration);
    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        targetRenderer.material.color = hoverColor;
    }

    private void SendHaptics(IXRInteractor interactor, float amplitude, float duration)
    {
        if (interactor == null) return;

        // Try get HapticSender from that controller
        HapticSender sender = interactor.transform.GetComponentInChildren<HapticSender>();

        if (sender != null)
        {
            sender.SendHapticImpulse(amplitude, duration);
        }
    }
}