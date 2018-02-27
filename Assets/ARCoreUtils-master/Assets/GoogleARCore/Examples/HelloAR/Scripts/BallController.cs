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

    private bool inHands;

    public float thrust;
    public float upthrust;
    public float torque;

    private Rigidbody rb;
    private TouchInput ti;
    private DistanceFromGoal distscript;
    private ControllerScript controller;

    public AudioSource ballthrow;
    public AudioSource impact;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        distscript = FindObjectOfType<DistanceFromGoal>();
        controller = FindObjectOfType<ControllerScript>();
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
                    if (couldbeswipe == true && swipeDist > comfortZone && tag == "inHands" && controller.isPaused == false) {
                        GetVelocity = false;
                        touchEnd = touch.position;
                        GetSpeed();
                        GetAngle();
                        rb.transform.parent = null;
                        rb.useGravity = true;
                        gameObject.GetComponent<SphereCollider>().enabled = true;
                        rb.AddForce((Camera.main.transform.forward * ballSpeed * distscript.dist * 2.1f));
                        if (distscript.dist <= 2.5f)
                        {
                            rb.AddForce(0, Mathf.Clamp((worldAngle.y * ballSpeed * distscript.dist * -.1f), 40, 100 * distscript.dist), 0);// * (worldAngle.x * ballSpeed * .05f) * (Mathf.Clamp(worldAngle.y * ballSpeed * .11f, 50, 80)));
                        } else
                        {
                            rb.AddForce(0, Mathf.Clamp((worldAngle.y * ballSpeed * distscript.dist * -.1f), 40, 250), 0);
                        }
                        ballthrow.Play();
                        tag = "basketball";
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
        }
    }

    void GetAngle()
    {
        worldAngle = Camera.main.ScreenToWorldPoint(new Vector3(touchEnd.x, touchEnd.y * 2, (Camera.main.nearClipPlane - 100)));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.y < -1 || rb.velocity.y > 1)
        {
            impact.Play();
        }
    }
}
        
