using UnityEngine;
using System.Collections;

public class CameraScale : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // Obtain the Camera component
        Camera camera = GetComponent<Camera>();

        // Set the desired FOV for the 9:16 aspect ratio
        float baseFOV = 60f; // FOV at 9:16 aspect ratio

        // Set the desired FOV for the target resolution (2960x1440)
        float targetResolutionFOV = 70f;

        // Calculate the aspect ratio of the screen
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Calculate the scale factor based on the screen width and the base aspect ratio of 9:16 (1.777...)
        float baseAspectRatio = 9f / 16f;
        float scaleFactor = windowAspect / baseAspectRatio;

        // Calculate the FOV adjustment
        float fovAdjustment = Mathf.Lerp(baseFOV, targetResolutionFOV, (scaleFactor - 1) / -0.15f);

        // Set the camera's FOV
        camera.fieldOfView = fovAdjustment;
    }

}
