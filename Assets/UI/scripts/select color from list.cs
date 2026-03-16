using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class selectcolorfromlist : MonoBehaviour
{


    public List<Color> colors;
    public void SetColor(int i)
    {
        GetComponent<Renderer>().material.color = colors[i];
    }




}
