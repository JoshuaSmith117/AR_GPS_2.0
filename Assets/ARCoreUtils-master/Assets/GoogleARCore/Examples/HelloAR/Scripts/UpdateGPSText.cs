using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using GoogleARCore.HelloAR;

public class UpdateGPSText : MonoBehaviour
{
    public Text coordinates;
    private void Start()
    {
    }
    private void Update()
    {
        coordinates.text = "Lat:" + GPS.Instance.latitude.ToString() + "   Long:" + GPS.Instance.longitude.ToString();
        if (GPS.Instance.inAssemblyHall == true)
        {
            coordinates.text += System.Environment.NewLine + "You are in Assmebly Hall";
        }
    }
}
