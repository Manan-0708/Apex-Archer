using UnityEngine;
using UnityEngine.XR;

public class XRResolutionFix : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRSettings.eyeTextureResolutionScale = 1.3f;
    }
}
