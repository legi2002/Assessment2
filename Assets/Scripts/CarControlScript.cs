using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based off the Unity manual for creating a car with wheel colliders

public class CarControl : MonoBehaviour
{
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float maxSteerAngle = 30;
    public float maxSteerAngleAtSpeed = 10;

    WheelCollider[] wheels;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.up * -1;

        wheels = GetComponentsInChildren<WheelCollider>();
    }

    void Update() {
        float throttleInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        float speed = Vector3.Dot(transform.forward, rb.velocity);

        float percentOfMaxSpeed = Mathf.InverseLerp(0, maxSpeed, speed);

        // Torque approaches 0 as car approaches max speed
        float currentTorque = Mathf.Lerp(motorTorque, 0, percentOfMaxSpeed);

        // Reduce steer angle as car approaches max speed
        float steerAngle = Mathf.Lerp(maxSteerAngle, maxSteerAngleAtSpeed, percentOfMaxSpeed);

        bool isAccelerating = Mathf.Sign(throttleInput) == Mathf.Sign(speed);

        foreach (var wheel in wheels) {
            if (wheel.GetComponent<WheelControlScript>().isSteerable) {
                wheel.steerAngle = steerInput * steerAngle;
            }

            if (isAccelerating) {
                if (wheel.GetComponent<WheelControlScript>().isDriveWheel) {
                    wheel.motorTorque = throttleInput * currentTorque;
                }
                wheel.brakeTorque = 0;
            } else {
                wheel.brakeTorque = Mathf.Abs(throttleInput) * brakeTorque;
                wheel.motorTorque = 0;
            }
        }
    }
}
