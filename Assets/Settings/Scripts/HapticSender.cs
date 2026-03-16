using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class HapticSender : MonoBehaviour
{
    [Tooltip("Default node to send haptics to if not provided.")]
    public XRNode node = XRNode.RightHand;

    // Send a single immediate impulse (safe: checks capabilities & device validity)
    public void SendHapticImpulse(float amplitude, float duration, XRNode? targetNode = null)
    {
        XRNode sendNode = targetNode ?? node;
        var device = InputDevices.GetDeviceAtXRNode(sendNode);
        if (!device.isValid) return;

        if (device.TryGetHapticCapabilities(out HapticCapabilities caps) && caps.supportsImpulse)
        {
            device.SendHapticImpulse(0u, Mathf.Clamp01(amplitude), duration);
        }
    }

    // Helper to send repeated short pulses (useful while holding / stretching)
    public IEnumerator PulseRepeated(float amplitude, float pulseDuration, float interval, float totalTime, XRNode? targetNode = null)
    {
        float elapsed = 0f;
        while (elapsed < totalTime)
        {
            SendHapticImpulse(amplitude, pulseDuration, targetNode);
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
    }
}