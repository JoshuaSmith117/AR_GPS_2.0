using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class BallController : MonoBehaviour
{
    public float thrust;
    public float upthrust;
    public float torque;

    private Rigidbody rb;
    private TouchInput ti;
    private DistanceFromGoal distscript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        distscript = GameObject.FindObjectOfType<DistanceFromGoal>();

    }

    void OnTouchDown()
    {
        //rb.AddForce(transform.up * -100);
    }
    void OnTouchUp()
    {
        rb.useGravity = true;
        rb.AddForce(transform.up * upthrust * distscript.dist);
        rb.AddForce(transform.forward * thrust * distscript.dist);
    }
    void OnTouchStay()
    {
        rb.useGravity = false;
    }
    void OnTouchExit()
    {
        rb.useGravity = true;
    }

    private void Update()
    {
        if (rb.transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
    }
}
