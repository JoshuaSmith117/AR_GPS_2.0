using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class BallController : MonoBehaviour
{
    Vector2 touchStart;
    Vector2 touchEnd;
    float flickTime = 0;
    float flickLength = 0;
    float ballVelocity;
    float ballSpeed = 0f;
    Vector3 worldAngle;
    private bool GetVelocity;
    public float comfortZone;
    private float swipeDist;
    bool couldbeswipe;
    float startCountdownLength = 0.0f;
    bool startTheTimer = false;
    static bool globalGameStart = false;
    static bool shootEnable = false;
    private float startGameTimer = 0.0f;

    public float thrust;
    public float upthrust;
    public float torque;

    private Rigidbody rb;
    private TouchInput ti;
    private DistanceFromGoal distscript;
    private ControllerScript controller;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        distscript = GameObject.FindObjectOfType<DistanceFromGoal>();
        controller = GameObject.FindObjectOfType<ControllerScript>();
    }

    void OnTouchDown()
    {

    }
    void OnTouchUp()
    {
        rb.useGravity = true;
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
        /*Debug.Log("camera.z = " + Camera.main.transform.rotation.z);
        
        if (worldAngle != null)
        {
            Debug.Log("worldAngle.z = " + worldAngle.z);
        }*/

        if (rb.transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    timeIncrease();
                    couldbeswipe = true;
                    GetVelocity = true;
                    touchStart = touch.position;
                    break;
                case TouchPhase.Moved:
                    if (Mathf.Abs(touch.position.y - touchStart.y) < comfortZone)
                    {
                        couldbeswipe = false;
                    }
                    else
                    {
                        couldbeswipe = true;
                    }
                    break;
                case TouchPhase.Stationary:
                    if (Mathf.Abs(touch.position.y - touchStart.y) < comfortZone)
                    {
                        couldbeswipe = false;
                    }
                    break;
                case TouchPhase.Ended:
                    swipeDist = (touch.position - touchStart).magnitude;
                    if (couldbeswipe == true && swipeDist > comfortZone) {
                        GetVelocity = false;
                        touchEnd = touch.position;
                        GetSpeed();
                        GetAngle();
                        rb.transform.parent = null;
                        rb.useGravity = true;
                        gameObject.GetComponent<SphereCollider>().enabled = true;
                        //rb.AddForce((worldAngle.x * ballSpeed * .05f) , (Mathf.Clamp(worldAngle.y * ballSpeed * .11f, 50, 80) * (distscript.dist)), (worldAngle.z * ballSpeed * .05f * distscript.dist));
                        rb.AddForce((Camera.main.transform.forward * ballSpeed * distscript.dist * 2));
                        rb.AddForce(0, (worldAngle.y * ballSpeed * distscript.dist * -.4f), 0);// * (worldAngle.x * ballSpeed * .05f) * (Mathf.Clamp(worldAngle.y * ballSpeed * .11f, 50, 80)));
                        controller.ballinhand = false;
                    }
                    break;
            }
            if (GetVelocity)
            {
                flickTime++;
            }
        }
    }

    void timeIncrease()
    {
        if (GetVelocity)
        {
            flickTime++;
        }
    }

    void GetSpeed()
    {
        if (flickTime > 0)
        {
            flickTime = flickTime * 10;
            ballSpeed = swipeDist / (swipeDist * flickTime);
            ballSpeed = ballSpeed * 1000f;
            Debug.Log("ballspeed = " + ballSpeed);
        }
    }

    void GetAngle()
    {
        worldAngle = Camera.main.ScreenToWorldPoint(new Vector3(touchEnd.x, touchEnd.y * 2, (Camera.main.nearClipPlane - 100)));
        //Debug.Log("worldAngle.x" + worldAngle.x);
    }
}
        
