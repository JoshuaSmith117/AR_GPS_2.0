using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.HelloAR;
using UnityEngine.UI;

public class DistanceFromGoal : MonoBehaviour {

    private ControllerScript controllerScript;
    private GameObject basketballGoal;

    public float dist = 0;
    public Text distText;

	// Use this for initialization
	void Start () {
        controllerScript = GameObject.FindObjectOfType<ControllerScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controllerScript.isGoalPlaced)
        {
            basketballGoal = GameObject.FindGameObjectWithTag("goal");
            dist = Vector3.Distance(basketballGoal.transform.position, transform.position);
            distText.text = "Distance to Goal: " + dist;
        }
    }
}
