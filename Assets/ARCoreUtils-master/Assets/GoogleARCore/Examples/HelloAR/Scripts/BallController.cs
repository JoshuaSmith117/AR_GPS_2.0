using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class BallController : MonoBehaviour
{

    private Rigidbody rb;
    public float distance;
    public float thrust;
    public float upthrust;
    public float torque;

    private TouchInput ti;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTouchDown()
    {
        //rb.AddForce(transform.up * -100);
    }
    void OnTouchUp()
    {
        rb.useGravity = true;
        rb.AddForce(transform.up * upthrust);
        rb.AddForce(transform.forward * thrust);
        //rb.AddTorque(transform.up * 1000f * Input.GetAxis("Horizontal") *);
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
