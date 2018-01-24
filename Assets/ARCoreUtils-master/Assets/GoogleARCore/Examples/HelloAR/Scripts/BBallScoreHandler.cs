using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class BBallScoreHandler : MonoBehaviour {

    public bool isGoalPlaced;

    public int score = 0;

    private Text scoretxt;

    private ParticleSystem goalParticles;

    private ControllerScript controller;

    // Use this for initialization
    void Start () {
        scoretxt = GameObject.Find("ScoreText").GetComponent<Text>();
        goalParticles = GameObject.Find("goalParticles").GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        scoretxt.text = "Score: " + score.ToString();
    }

    void OnTriggerExit(Collider other)
    {
        score += 1;
        goalParticles.Play();
        Debug.Log("GOAL!");
        Debug.Log("score handler score: " + score);

    }

}
