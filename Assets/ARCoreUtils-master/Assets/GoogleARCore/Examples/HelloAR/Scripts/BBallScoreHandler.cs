using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class BBallScoreHandler : MonoBehaviour {

    public bool isGoalPlaced;
    [HideInInspector] public bool primed;

    public int score = 0;

    private Text scoretxt;
    private Text shotvaluetxt;

    private ParticleSystem goalParticles;

    private GameObject basketballHoop;

    private GameObject ppObject;
    public GameObject ppPrefab;

    private ControllerScript controller;
    private DistanceFromGoal distscript;

    // Use this for initialization
    void Start () {
        distscript = FindObjectOfType<DistanceFromGoal>();
        scoretxt = GameObject.Find("ScoreNum").GetComponent<Text>();
        shotvaluetxt = GameObject.Find("ShotValueNum").GetComponent<Text>();
        goalParticles = GameObject.Find("goalParticles").GetComponent<ParticleSystem>();
        basketballHoop = GameObject.FindGameObjectWithTag("hoop");
    }
	
	// Update is called once per frame
	void Update () {
        scoretxt.text = score.ToString();
        shotvaluetxt.text = ("+" + distscript.shotvalue.ToString());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.position.y >= transform.position.y)
        {
            IEnumerator coroutine = Prime();
            StartCoroutine(coroutine);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.position.y <= transform.position.y)
        {
            if (primed == true)
            {
                score += distscript.shotvalue;
                goalParticles.Play();
                ppObject = Instantiate(ppPrefab,basketballHoop.transform);
                ppObject.GetComponent<Text>().text = "+" + distscript.shotvalue;
                ppObject.GetComponent<Rigidbody>().AddForce(Random.Range(-20f, 20f), Random.Range(40f, 80f), 0);
                IEnumerator coroutine = Despawn();
                StartCoroutine(coroutine);
            }
        }
    }
    IEnumerator Prime() {
        primed = true;
        yield return new WaitForSeconds(1);
        primed = false;
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(1);
        Destroy(ppObject);
    }
}
