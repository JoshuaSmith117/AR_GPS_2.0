using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class BallController : MonoBehaviour
{
    Vector2 touchStart;
    Vector2 touchEnd;
    int flickTime = 5;
    int flickLength = 0;
    float ballVelocity;
    float ballSpeed = 0;
    Vector3 worldAngle;
    private bool GetVelocity;
    float comfortZone;
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
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    flickTime = 5;
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
                    var swipeDist = (touch.position - touchStart).magnitude;
                    if (couldbeswipe == true && swipeDist > comfortZone) {
                        GetVelocity = false;
                        touchEnd = touch.position;
                        GetSpeed();
                        GetAngle();
                        rb.transform.parent = null;
                        rb.useGravity = true;
                        rb.AddForce(new Vector3((worldAngle.x * ballSpeed), (worldAngle.y * ballSpeed), (worldAngle.z * ballSpeed)));
                        rb.AddForce(transform.up * 30);
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
        flickLength = 90;
        if (flickTime > 0)
        {
            ballVelocity = flickLength / (flickLength - flickTime);
        }
        ballSpeed = ballVelocity * 2;
        ballSpeed = ballSpeed - (ballSpeed * 1.65f);
        if (ballSpeed <= -33)
        {
            ballSpeed = -33;
        }
        Debug.Log("flick was" + flickTime);
        flickTime = 5;
    }

    void GetAngle()
    {
        worldAngle = Camera.main.ScreenToWorldPoint(new Vector3(touchEnd.x, touchEnd.y + 100, ((Camera.main.nearClipPlane - 100) * 1.8f)));
    }
}
        
