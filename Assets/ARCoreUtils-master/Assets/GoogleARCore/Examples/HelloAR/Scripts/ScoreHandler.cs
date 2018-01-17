using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {
    
    private int score = 0;
    private Text scoretxt;
    private ParticleSystem goalParticles;

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
        //if (collision.gameObject.name == "basketball"){
        score += 1;
        goalParticles.Play();
        Debug.Log("GOAL!");
        
        //}

    }

}
