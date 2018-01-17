using GoogleARCore.HelloAR;
using GoogleARCore;
using System.Collections.Generic;
using UnityEngine;

public class ARSurfaceManager : MonoBehaviour
{
    private ControllerScript controlScript;
    private GameObject surfaceObj;

    [SerializeField] Material m_surfaceMaterial;
    [SerializeField] Material grid;
    [SerializeField] Material ARSurfaceMat;
    List<TrackedPlane> m_newPlanes = new List<TrackedPlane>();

    private void Start()
    {
        controlScript = GameObject.FindObjectOfType<ControllerScript>();
        surfaceObj = null;
    }

    void Update()
	{
#if UNITY_EDITOR
		return;
#endif

		if (Frame.TrackingState != TrackingState.Tracking)
		{
			return;
		}

		Frame.GetPlanes(m_newPlanes, TrackableQueryFilter.New);

		foreach (var plane in m_newPlanes)
		{
			surfaceObj = new GameObject("ARSurface");
            surfaceObj.AddComponent<ARSurface>().SetTrackedPlane(plane, grid);
        }

        if (controlScript.goals.Length <= 0)
        {
            Debug.Log(controlScript.goals.Length);
            surfaceObj.GetComponent<ARSurface>().GetComponent<Renderer>().material = grid;
            Debug.Log(surfaceObj.GetComponent<ARSurface>().GetComponent<Renderer>().material);
        }
        else if (controlScript.goals.Length >= 1)
        {
            Debug.Log(controlScript.goals.Length);
            surfaceObj.GetComponent<ARSurface>().GetComponent<Renderer>().material = ARSurfaceMat;
            Debug.Log(surfaceObj.GetComponent<ARSurface>().GetComponent<Renderer>().material);
        }
    }
}
