using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class BBallScoreHandler : MonoBehaviour {

    public bool isGoalPlaced;

    public int score = 0;

    private Text scoretxt;
    private Text shotvaluetxt;

    private ParticleSystem goalParticles;

    private ControllerScript controller;
    private DistanceFromGoal distscript;

    // Use this for initialization
    void Start () {
        distscript = GameObject.FindObjectOfType<DistanceFromGoal>();
        scoretxt = GameObject.Find("ScoreNum").GetComponent<Text>();
        shotvaluetxt = GameObject.Find("ShotValueNum").GetComponent<Text>();
        goalParticles = GameObject.Find("goalParticles").GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        scoretxt.text = score.ToString();
        shotvaluetxt.text = ("+" + distscript.shotvalue.ToString());
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("in");
        score += distscript.shotvalue;
        Debug.Log(distscript.shotvalue);
        goalParticles.Play();
        Debug.Log("GOAL!");
        Debug.Log("score handler score: " + score);
    }

}
