using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based off the Unity manual for creating a car with wheel colliders

public class WheelControlScript : MonoBehaviour
{
    public Transform wheelTransform;
    public WheelCollider wheelCollider;

    public bool isSteerable;
    public bool isDriveWheel;

    Vector3 pos;
    Quaternion rotation;
    
    void Start() {
        wheelCollider = GetComponent<WheelCollider>();
    }

    void Update() {
        wheelCollider.GetWorldPose(out pos, out rotation);
        wheelTransform.transform.position = pos;
        wheelTransform.transform.rotation = rotation;
    }
}
