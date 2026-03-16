using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BowStringController : MonoBehaviour
{
    [SerializeField] private BowString bowStringRenderer;
    [SerializeField] private Transform midPointGrabObject;
    [SerializeField] private Transform midPointVisualObject;
    [SerializeField] private Transform midPointParent;
    [SerializeField] private float bowStringStretchLimit = 0.6f;

    [Header("Mesh String")]
    [SerializeField] private Transform stringMesh;
    [SerializeField] private Vector3 stringRestPosition = Vector3.zero;

    [Header("Spring Effect")]
    [SerializeField] private float springStiffness = 30f;
    [SerializeField] private float springDamping = 4f;
    [SerializeField] private float releaseImpulse = 2.5f;

    [Header("Haptics")]
    [SerializeField] private HapticSender hapticsFallback;

    private HapticSender currentHaptics;
    private float lastPulseTime;
    private XRGrabInteractable interactable;
    private Transform interactor;
    private float strength;

    // Spring simulation
    private float springVelocity = 0f;
    private float currentMidZ = 0f;
    private bool isOscillating = false;
    private bool isBeingPulled = false;

    public UnityEvent OnBowPulled;
    public UnityEvent<float> OnBowReleased;

    private void Awake()
    {
        interactable = midPointGrabObject
            .GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        interactable.selectEntered
            .AddListener(PrepareBowString);
        interactable.selectExited
            .AddListener(ResetBowString);

        // Save string mesh rest position
        if (stringMesh != null)
            stringRestPosition =
                stringMesh.localPosition;
    }

    private void PrepareBowString(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject.transform;
        currentHaptics = args.interactorObject.transform
            .GetComponentInChildren<HapticSender>()
            ?? hapticsFallback;

        isBeingPulled = true;
        isOscillating = false;
        springVelocity = 0f;

        OnBowPulled?.Invoke();
        SessionTracker.Instance.OnBowGrab();
        TutorialController.Instance?.OnBowGrab();
    }

    private void ResetBowString(SelectExitEventArgs args)
    {
        // Fire haptic snap
        currentHaptics?.SendHapticImpulse(1f, 0.12f);

        // Fire arrow
        OnBowReleased?.Invoke(strength);

        // Launch spring forward
        springVelocity = releaseImpulse
            * Mathf.Abs(currentMidZ) * 10f;
        isOscillating = true;
        isBeingPulled = false;

        strength = 0f;
        currentHaptics = null;
        interactor = null;
        midPointGrabObject.localPosition = Vector3.zero;

        SessionTracker.Instance.OnBowRelease();
    }

    private void Update()
    {
        if (isBeingPulled && interactor != null)
            HandlePulling();

        if (isOscillating)
            HandleSpring();

        TutorialController.Instance?.OnBowPulled(strength);
        bowStringRenderer.CreateString(
            midPointVisualObject.position);
    }

    private void HandlePulling()
    {
        Vector3 grabLocal = midPointParent
            .InverseTransformPoint(
                midPointGrabObject.position);

        float localZ = Mathf.Clamp(
            grabLocal.z,
            -bowStringStretchLimit,
            0f);

        float pullAbs = Mathf.Abs(localZ);

        if (localZ < 0f && pullAbs > 0f)
        {
            float normalized = Mathf.Clamp01(
                pullAbs / bowStringStretchLimit);
            strength = normalized * normalized;
        }
        else
        {
            strength = 0f;
        }

        // Haptic pulse while pulling
        if (currentHaptics != null
            && Time.time - lastPulseTime > 0.05f)
        {
            currentHaptics.SendHapticImpulse(
                Mathf.Clamp01(strength * 0.5f), 0.02f);
            lastPulseTime = Time.time;
        }

        currentMidZ = localZ;
        UpdateVisuals();
    }

    private void HandleSpring()
    {
        // Spring force pulls toward 0
        float springForce =
            -springStiffness * currentMidZ;

        // Damping slows oscillation
        float dampingForce =
            -springDamping * springVelocity;

        // Apply forces
        springVelocity += (springForce + dampingForce)
            * Time.deltaTime;
        currentMidZ += springVelocity * Time.deltaTime;

        // Clamp so string never crosses bow mesh
        currentMidZ = Mathf.Clamp(
            currentMidZ, -0.15f, 0.15f);

        UpdateVisuals();

        // Settled — stop oscillating
        if (Mathf.Abs(currentMidZ) < 0.001f
            && Mathf.Abs(springVelocity) < 0.001f)
        {
            currentMidZ = 0f;
            springVelocity = 0f;
            isOscillating = false;

            // Reset both visuals to rest
            midPointVisualObject.localPosition =
                                    new Vector3(0f, 0f, 0f);

            if (stringMesh != null)
                stringMesh.localPosition =
                    stringRestPosition;
        }
    }

    private void UpdateVisuals()
    {
        // Update LineRenderer midpoint
        if (midPointVisualObject.parent == midPointParent)
            midPointVisualObject.localPosition =
                new Vector3(0f, 0f, currentMidZ);
        else
            midPointVisualObject.position =
                midPointParent.TransformPoint(
                    new Vector3(-currentMidZ, 0f, 0f));

        // Update string mesh position
        if (stringMesh != null)
            stringMesh.localPosition = new Vector3(
            stringRestPosition.x - currentMidZ,
            stringRestPosition.y,
            stringRestPosition.z);
    }
}