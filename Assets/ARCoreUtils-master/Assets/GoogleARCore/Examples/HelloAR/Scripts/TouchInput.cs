using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class TouchInput : MonoBehaviour
{
    private bool touchable;
    private Controller Controller;
    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld;
    private Vector3 position;
    private Vector3 touchPosition;

    public LayerMask touchInputMask;
    public RaycastHit hit;
    public Camera cam;

    void Start()
    {
        Debug.Log("start");
        cam = GetComponent<Camera>();
        touchable = true;
        //touchable = Controller.canTouch;
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (touchable == true)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {

                touchesOld = new GameObject[touchList.Count];
                touchList.CopyTo(touchesOld);
                touchList.Clear();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, touchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    Rigidbody rb = recipient.GetComponent<Rigidbody>();
                    touchList.Add(recipient);

                    if (Input.GetMouseButtonDown(0))
                    {
                        recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetMouseButton(0))
                    {
                        position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));

                        //touchPosition = Vector3.Lerp(hit.rigidbody.position, position, .1f);
                        hit.rigidbody.transform.position = position;
                    }
                }
                foreach (GameObject g in touchesOld)
                {
                    if (!touchList.Contains(g))
                    {
                        g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
#endif
        if (touchable == true)
        {
            if (Input.touchCount > 0)
            {

                touchesOld = new GameObject[touchList.Count];
                touchList.CopyTo(touchesOld);
                touchList.Clear();

                foreach (Touch touch in Input.touches)
                {

                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out hit, touchInputMask))
                    {
                        GameObject recipient = hit.transform.gameObject;
                        touchList.Add(recipient);

                        if (touch.phase == TouchPhase.Began)
                        {
                            recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                        }
                        if (touch.phase == TouchPhase.Ended)
                        {
                            recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                        }
                        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                        {
                            position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1));
                            hit.rigidbody.transform.position = position;
                            hit.rigidbody.transform.rotation = Camera.main.transform.rotation;
                            recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                        }
                        if (touch.phase == TouchPhase.Canceled)
                        {
                            hit.rigidbody.useGravity = true;
                            recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
                foreach (GameObject g in touchesOld)
                {
                    if (!touchList.Contains(g))
                    {
                        g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }
}
