using UnityEngine;

public class FloatingPanel : MonoBehaviour
{
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        float floatAmount = Mathf.Sin(Time.unscaledTime * 1.5f) * 0.02f;
        transform.localPosition = startPos + Vector3.up * floatAmount;
    }
}
