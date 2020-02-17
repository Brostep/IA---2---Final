using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabController : MonoBehaviour
{
    public Canvas soldCanvas;

    private void Start()
    {
        soldCanvas.enabled = false;
    }

    public void SellLab()
    {
        soldCanvas.enabled = true;
    }
}
