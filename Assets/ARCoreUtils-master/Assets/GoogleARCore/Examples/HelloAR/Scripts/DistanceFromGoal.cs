using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.HelloAR;
using UnityEngine.UI;

public class DistanceFromGoal : MonoBehaviour {
    private ControllerScript controllerScript;
    private GameObject basketballGoal;
    private float dist;
    public Text distText;

	// Use this for initialization
	void Start () {
        controllerScript = GameObject.FindObjectOfType<ControllerScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controllerScript.isGoalPlaced == true)
        {
            basketballGoal = GameObject.Find("BasketballGoal");
            dist = Vector3.Distance(basketballGoal.transform.position, transform.position);
            distText.text = "Distance to Goal: " + dist;
            Debug.Log(dist);
        }
    }
}
