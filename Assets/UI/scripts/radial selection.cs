using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RadialSelection : MonoBehaviour
{
    // OpenXR Input Action
    public InputActionProperty spawnButton;

    [Range(2, 10)]
    public int numberofradialparts;

    public GameObject radialpartprefab;
    public Transform radialpartcanvas;
    public float anglebetweenradialparts = 10;
    public Transform HandTransform;
    public UnityEvent<int> onpartselected;

    private List<GameObject> spawnedparts = new List<GameObject>();
    private int CurrentSelectedRadialpart = -1;

    void Update()
    {
        if (spawnButton.action == null) return;

        // Button Pressed
        if (spawnButton.action.WasPressedThisFrame())
        {
            SpawnRadialParts();
        }

        // Button Held
        if (spawnButton.action.IsPressed())
        {
            GetSelectedRadialPart();
        }

        // Button Released
        if (spawnButton.action.WasReleasedThisFrame())
        {
            HideandTriggerSelected();
        }
    }

    public void HideandTriggerSelected()
    {
        onpartselected?.Invoke(CurrentSelectedRadialpart);
        radialpartcanvas.gameObject.SetActive(false);
    }

    public void GetSelectedRadialPart()
    {
        Vector3 centertohand = HandTransform.position - radialpartcanvas.position;
        Vector3 centertohandprojected =
            Vector3.ProjectOnPlane(centertohand, radialpartcanvas.forward);

        float angle = Vector3.SignedAngle(
            radialpartcanvas.up,
            centertohandprojected,
            radialpartcanvas.forward
        );

        if (angle < 0)
            angle += 360;

        Debug.Log("Angle: " + angle);

        CurrentSelectedRadialpart =
            (int)(angle * numberofradialparts / 360);

        for (int i = 0; i < spawnedparts.Count; i++)
        {
            if (i == CurrentSelectedRadialpart)
            {
                spawnedparts[i].GetComponent<Image>().color = Color.yellow;
                spawnedparts[i].transform.localScale = Vector3.one * 1.1f;
            }
            else
            {
                spawnedparts[i].GetComponent<Image>().color = Color.white;
                spawnedparts[i].transform.localScale = Vector3.one;
            }
        }
    }

    public void SpawnRadialParts()
    {
        radialpartcanvas.gameObject.SetActive(true);
        radialpartcanvas.position = HandTransform.position;
        radialpartcanvas.rotation = HandTransform.rotation;

        foreach (var item in spawnedparts)
        {
            Destroy(item);
        }

        spawnedparts.Clear();

        for (int i = 0; i < numberofradialparts; i++)
        {
            float angle =
                i * 360f / numberofradialparts -
                anglebetweenradialparts / 2f;

            Vector3 radialparteulerangle =
                new Vector3(0, 0, angle);

            GameObject spawnradialpart =
                Instantiate(radialpartprefab, radialpartcanvas);

            spawnradialpart.transform.localEulerAngles =
                radialparteulerangle;

            spawnradialpart.GetComponent<Image>().fillAmount =
                (1f / numberofradialparts) -
                (anglebetweenradialparts / 360f);

            spawnedparts.Add(spawnradialpart);
        }
    }
}
