using System;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ArrowLogger : MonoBehaviour
{
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        if (grab == null)
        {
            Debug.LogError("XRGrabInteractable missing on Arrow!");
            enabled = false;
            return;
        }
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

        VRCSVLogger.Log(
            "ArrowGrabbed",
            gameObject.name,
            hand
        );
    }

    void OnRelease(SelectExitEventArgs args)
    {
        string hand = args.interactorObject.transform.name;

        VRCSVLogger.Log(
            "ArrowReleased",
            gameObject.name,
            hand
        );
    }
}
