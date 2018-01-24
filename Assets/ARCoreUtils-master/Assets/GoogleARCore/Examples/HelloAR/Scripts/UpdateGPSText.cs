using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using GoogleARCore.HelloAR;

public class UpdateGPSText : MonoBehaviour
{
    private ControllerScript control;

    public Text coordinates;
    private void Start()
    {
        control = FindObjectOfType<ControllerScript>();
    }
    private void Update()
    {
        if (control.hasBegun == true)
        {
            coordinates.enabled = false;
        } else if (control.hasBegun == false)
        {
            coordinates.enabled = true;
            coordinates.text = "Lat:" + GPS.Instance.latitude.ToString() + "   Long:" + GPS.Instance.longitude.ToString();
            if (GPS.Instance.inAssemblyHall == true)
            {
                coordinates.text += System.Environment.NewLine + "You are in Assembly Hall";
            }
        }
    }
}
