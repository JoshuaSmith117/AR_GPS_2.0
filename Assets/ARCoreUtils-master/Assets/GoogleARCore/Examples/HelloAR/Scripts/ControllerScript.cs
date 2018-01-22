//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.UI;

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class ControllerScript : MonoBehaviour
    {
        public Text coordinates;
        public Text TipText;
        public Text Timer;
        public Text Instruction;

        public Button playBtn;

        public bool canTouch = true;
        public bool isGoalPlaced;
        public bool isPlaying;
        public bool hasBegun;
        bool searchingForSurfaces;

        public float timeLeft;

        private GameObject[] basketballs;
        public GameObject[] goals;
        public GameObject startMenu;
        public GameObject welcomeMenu;

        private ParticleSystem goalPlaceParticles;

        private ScoreHandler scoreHandler;

        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject TrackedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject AndyAndroidPrefab;
        public GameObject BasketballPrefab;
        public GameObject BasketballGoalPrefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject TipUI;

        /// <summary>
        /// A list to hold new planes ARCore began tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<TrackedPlane> m_NewPlanes = new List<TrackedPlane>();

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<TrackedPlane> m_AllPlanes = new List<TrackedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        /// <summary>
        /// True if the detected planes should be visualized with transparent meshes.
        /// </summary>
        public bool VisualizePlanes = false;

        private void Start()
        {
            goalPlaceParticles = GameObject.Find("goalParticles").GetComponent<ParticleSystem>();
            scoreHandler = FindObjectOfType<ScoreHandler>();
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            basketballs = GameObject.FindGameObjectsWithTag("basketball");
            goals = GameObject.FindGameObjectsWithTag("goal");

            if (hasBegun == true)
            {
                if (searchingForSurfaces == true)
                {
                    Instruction.text = "Please scan the ground with your camera.";
                } else {
                    Instruction.enabled = false;
                }
            }

            if (isPlaying == true)
            {
                timeLeft -= Time.deltaTime;
                timeLeft = Mathf.Round(timeLeft * 100f) / 100f;
                Timer.text = "Timer: " + timeLeft;
                if (timeLeft <= 0)
                {
                    GameOver();
                }
            } else if (isPlaying == false)
            {
                Timer.enabled = false;
            }

            if (goals.Length <= 0)
            {
                isGoalPlaced = false;
                TipUI.SetActive(true);
                if (isPlaying == false)
                {
                    startMenu.SetActive(false);
                }
            } else if (goals.Length >= 1)
            {
                isGoalPlaced = true;
                TipUI.SetActive(false);
                if (isPlaying == false)
                {
                    startMenu.SetActive(true);
                }
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            _QuitOnConnectionErrors();

            // Check that motion tracking is tracking.
            if (Frame.TrackingState != TrackingState.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
                return;
            }

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
            if (VisualizePlanes)
            {
                Frame.GetPlanes(m_NewPlanes, TrackableQueryFilter.New);
                for (int i = 0; i < m_NewPlanes.Count; i++)
                {
                    // Instantiate a plane visualization prefab and set it to track the new plane. The transform is set to
                    // the origin with an identity rotation since the mesh for our prefab is updated in Unity World
                    // coordinates.
                    GameObject planeObject = Instantiate(TrackedPlanePrefab, Vector3.zero, Quaternion.identity,
                        transform);
                    planeObject.GetComponent<TrackedPlaneVisualizer>().Initialize(m_NewPlanes[i]);
                }
            }

            // Disable the snackbar UI when no planes are valid.
            Frame.GetPlanes(m_AllPlanes);
            searchingForSurfaces = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    searchingForSurfaces = false;
                    TipText.text = "Tap to place a basketball goal.";
                    break;
                }
                else {
                    searchingForSurfaces = true;
                    TipText.text = "Searching for surfaces...";
                    break;
                }
            }

            if (GPS.Instance.inAssemblyHall == true)
            {
                if (basketballs.Length <= 2)
                {

                    // If the player has not touched the screen, we are done with this update.
                    Touch touch;
                    if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                    {
                        return;
                    }

                    // Raycast against the location the player touched to search for planes.
                    TrackableHit hit;
                    TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon;

                    if (Session.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
                    {
                        if (isGoalPlaced == false && hasBegun == true)
                        {
                            var basketballGoalObject = Instantiate(BasketballGoalPrefab, hit.Pose.position, hit.Pose.rotation);
                            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                            // world evolves.
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                            // Andy should look at the camera but still be flush with the plane.
                            //basketballObject.transform.LookAt(FirstPersonCamera.transform);
                            basketballGoalObject.transform.rotation = Quaternion.Euler(0.0f,
                            basketballGoalObject.transform.rotation.eulerAngles.y, basketballGoalObject.transform.rotation.z);

                            // Make Andy model a child of the anchor.
                            basketballGoalObject.transform.parent = anchor.transform;
                            Debug.Log("Goal has been placed.");
                        }

                        else if (isGoalPlaced == true)
                        {
                            var basketballObject = Instantiate(BasketballPrefab, hit.Pose.position, hit.Pose.rotation);

                            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                            // world evolves.
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                            // Andy should look at the camera but still be flush with the plane.
                            //basketballObject.transform.LookAt(FirstPersonCamera.transform);
                            basketballObject.transform.rotation = Quaternion.Euler(0.0f,
                            basketballObject.transform.rotation.eulerAngles.y, basketballObject.transform.rotation.z);

                            // Make Andy model a child of the anchor.
                            basketballObject.transform.parent = anchor.transform;
                        }
                    }
                }
            }
        }

        public void Begin()
        {
            hasBegun = true;
        }

        public void PlayGame()
        {
            isPlaying = true;
            startMenu.SetActive(false);
            Timer.enabled = true;
            scoreHandler.score = 0;
        }

        public void GameOver()
        {
            isPlaying = false;
            startMenu.SetActive(true);
            Timer.enabled = false;
            timeLeft = 60f;
        }

        /// <summary>
        /// Quit the application if there was a connection error for the ARCore session.
        /// </summary>
        private void _QuitOnConnectionErrors()
        {
            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.ConnectionState == SessionConnectionState.UserRejectedNeededPermission)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("DoQuit", 0.5f);
            }
            else if (Session.ConnectionState == SessionConnectionState.ConnectToServiceFailed)
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
