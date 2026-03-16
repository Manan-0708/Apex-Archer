using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BowGrabLogger : MonoBehaviour
{
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        string hand = args.interactorObject.transform.name;
        VRCSVLogger.Log("BowGrabbed", gameObject.name, hand);

        SessionTracker.Instance.OnBowGrab();
    }

    void OnRelease(SelectExitEventArgs args)
    {
        string hand = args.interactorObject.transform.name;
        VRCSVLogger.Log("BowReleased", gameObject.name, hand);

        SessionTracker.Instance.OnBowRelease();
    }
}
