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
                        rb.AddForce((worldAngle.x * ballSpeed * .04f), (worldAngle.y * ballSpeed * .02f * distscript.dist), (worldAngle.z * ballSpeed * .02f * distscript.dist));
                        Debug.Log("x" + worldAngle.x * ballSpeed * .05f);
                        Debug.Log("y" + worldAngle.y * ballSpeed * .05f);
                        Debug.Log("z" + worldAngle.z * ballSpeed * .05f);
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
        }
        ballSpeed = ballSpeed * -1000f;
    }

    void GetAngle()
    {
        worldAngle = Camera.main.ScreenToWorldPoint(new Vector3(touchEnd.x, touchEnd.y * 2, ((Camera.main.nearClipPlane - 100) * 1.8f)));
        Debug.Log("touchend.y" + touchEnd.y);
    }
}
        
