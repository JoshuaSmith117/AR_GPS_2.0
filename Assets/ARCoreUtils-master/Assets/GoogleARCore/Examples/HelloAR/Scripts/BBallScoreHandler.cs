using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class BBallScoreHandler : MonoBehaviour {

    public bool isGoalPlaced;
    private bool primed;

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

    void OnTriggerEnter(Collider other)
    {
        if (this.name == "goalTriggerTop")
        {
            Debug.Log("IN TOP");
            IEnumerator coroutine = Prime();
            StartCoroutine(coroutine);
        }
        else if (this.name == "goalTriggerBottom") {
            Debug.Log("IN BOTTOM");
            if (primed == true)
            {
                score += distscript.shotvalue;
                Debug.Log(distscript.shotvalue);
                goalParticles.Play();
                Debug.Log("GOAL!");
                Debug.Log("score handler score: " + score);
            }
            
        }
    }
    IEnumerator Prime() {
        primed = true;
        Debug.Log(primed);
        yield return new WaitForSeconds(1);
        primed = false;
        Debug.Log(primed);
    }
}
