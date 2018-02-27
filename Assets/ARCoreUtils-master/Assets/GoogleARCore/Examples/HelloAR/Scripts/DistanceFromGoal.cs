using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.HelloAR;
using UnityEngine.UI;

public class DistanceFromGoal : MonoBehaviour {

    private ControllerScript controllerScript;
    private GameObject basketballHoop;

    public float dist = 0;
    public Text distText;
    public int shotvalue;

	// Use this for initialization
	void Start () {
        controllerScript = GameObject.FindObjectOfType<ControllerScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controllerScript.isGoalPlaced)
        {
            basketballHoop = GameObject.FindGameObjectWithTag("hoop");
            dist = Vector3.Distance(basketballHoop.transform.position, transform.position);
            distText.text = "Distance to Goal: " + dist;

            if (dist >= 3.6f)
            {
                shotvalue = 3;
            }
            else if (dist >= 2.6f)
            {
                shotvalue = 2;
            }
            else
            {
                shotvalue = 1;
            }
        }
    }
}
