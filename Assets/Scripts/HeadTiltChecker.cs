using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HeadTiltChecker : MonoBehaviour
{
    public ARFaceManager Face_Manager;

    public float Tilt_Angle_Threshold = 15f; // adjust as needed
    public int Tilt_Side = 0; // variable to hold head tilt side

    void Update()
    {
        if (Face_Manager.facePrefab != null)
        {
            foreach (var face in Face_Manager.trackables)
            {
                if (face.trackingState == TrackingState.Tracking)
                {
                    Quaternion faceRotation = face.transform.rotation;
                    Vector3 eulerAngles = faceRotation.eulerAngles;
                    if (eulerAngles.z > Tilt_Angle_Threshold && eulerAngles.z < 180f)
                    {
                        Tilt_Side = 2; // 2 for right side
                    }
                    else if (eulerAngles.z < 360f - Tilt_Angle_Threshold && eulerAngles.z > 180f)
                    {
                        Tilt_Side = 1; // 1 for left side
                    }
                    else
                    {
                        Tilt_Side = 0; // 0 if face is not tilted
                    }
                    break;
                }
            }
        }
    }

}