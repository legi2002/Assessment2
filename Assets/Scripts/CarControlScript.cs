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

    public AudioSource engineSound;
    public AudioSource brakeSound;

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

        if (speed < -0.1) engineSound.pitch = 1.5f;

        brakeSound.volume = 0;

        foreach (var wheel in wheels) {
            WheelControlScript wheelParams = wheel.GetComponent<WheelControlScript>();
            if (wheelParams.isSteerable) {
                wheel.steerAngle = steerInput * steerAngle;
            }

            if (isAccelerating) {
                if (wheelParams.isDriveWheel) {
                    wheel.motorTorque = throttleInput * currentTorque;
                }
                wheel.brakeTorque = 0;
            } else {
                wheel.brakeTorque = Mathf.Abs(throttleInput) * brakeTorque;
                wheel.motorTorque = 0;
            }

            // Detect wheel slip
            if (wheelParams.isDriveWheel) {
                ParticleSystem tireSmoke = wheel.GetComponentInChildren<ParticleSystem>();
                wheel.GetGroundHit(out WheelHit wheelHit);

                bool tireSlipping = false;
                float pitch = 1f;

                Debug.Log(wheelHit.sidewaysSlip);
                if (Math.Abs(wheelHit.sidewaysSlip) > 0.1) {
                    tireSlipping = true;
                    brakeSound.volume = 0.2f;
                }

                // Slip at low forward speed
                // Engine sound controlled here so it can max out if slipping
                if (throttleInput > 0 && speed < 10) {
                    tireSlipping = true;
                    pitch = 2.5f;
                    brakeSound.volume = 0.2f;
                } else {
                    pitch = 2 * percentOfMaxSpeed + 0.5f;
                }
                engineSound.pitch = Mathf.Lerp(engineSound.pitch, pitch, Time.deltaTime);

                // Slip when braking
                if (throttleInput < 0 && speed > 0) {
                    tireSlipping = true;
                    brakeSound.volume = 0.2f;
                }

                if (tireSlipping) {
                    tireSmoke.Play();
                } else {
                    tireSmoke.Stop();
                }
            }
        }
    }
}
