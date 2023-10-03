using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public float lookSensitivity;
    public float minXLook;
    public float maxXLook;
    public Transform camAnchor;

    public bool invertXRotation;

    private float curXRot;

    void Start ()
    {
        // lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // called at the end of each frame
    void LateUpdate ()
    {
        // get the mouse X and Y inputs
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        // rotate the player horizontally
        transform.eulerAngles += Vector3.up * x * lookSensitivity;

        // do we invert the vertical rotation?
        if(invertXRotation)
            curXRot += y * lookSensitivity;
        else
            curXRot -= y * lookSensitivity;

        // clamp the vertical rotation
        curXRot = Mathf.Clamp(curXRot, minXLook, maxXLook);

        // apply it to the camera
        Vector3 clampedAngle = camAnchor.eulerAngles;
        clampedAngle.x = curXRot;
        camAnchor.eulerAngles = clampedAngle;
    }
}