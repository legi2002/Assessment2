using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public AudioSource squealSound;

    public GameOverScript Over;

    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.up * -1;

        wheels = GetComponentsInChildren<WheelCollider>();
    }

    void Update() {
        // Inputs
        float throttleInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        // Speed related calculations
        float speed = Vector3.Dot(transform.forward, rb.velocity);
        float percentOfMaxSpeed = Mathf.InverseLerp(0, maxSpeed, speed);
        bool isAccelerating = Mathf.Sign(throttleInput) == Mathf.Sign(speed);

        // Torque approaches 0 as car approaches max speed
        float currentTorque = Mathf.Lerp(motorTorque, 0, percentOfMaxSpeed);
        // Reduce steer angle as car approaches max speed
        float steerAngle = Mathf.Lerp(maxSteerAngle, maxSteerAngleAtSpeed, percentOfMaxSpeed);

        foreach (var wheel in wheels) {
            WheelControlScript wheelParams = wheel.GetComponent<WheelControlScript>();

            if (wheelParams.isSteerable) {
                wheel.steerAngle = steerInput * steerAngle;
            }

            if (isAccelerating) {
                // Drive drivewheels
                if (wheelParams.isDriveWheel) {
                    wheel.motorTorque = throttleInput * currentTorque;
                }
                wheel.brakeTorque = 0;
            } else {
                // If car isn't accelerating then 'throttle' input is actually brake
                wheel.brakeTorque = Mathf.Abs(throttleInput) * brakeTorque;
                wheel.motorTorque = 0;
            }

            // Detect wheel slip and handle audio and effects
            if (wheelParams.isDriveWheel) {
                ParticleSystem tireSmoke = wheel.GetComponentInChildren<ParticleSystem>();
                wheel.GetGroundHit(out WheelHit wheelHit);
                bool tireSlipping = false;

                if (Math.Abs(wheelHit.sidewaysSlip) > 0.2) {
                    tireSlipping = true;
                }
                // Slip at low forward speed (not neccessarily slipping, but used for tire smoke and audio)
                if (throttleInput > 0 && speed < 10) {
                    tireSlipping = true;
                }
                // Slip when braking
                if (throttleInput < 0 && speed > 0) {
                    tireSlipping = true;
                }

                //--------------------Audio & Effects----------------------

                float targetPitch = 2 * percentOfMaxSpeed + 0.5f;

                if (tireSlipping) {
                    // Tire slipping whilst giving throttle means engine is revving high so max pitch
                    if (throttleInput > 0) targetPitch = 2.5f;
                    squealSound.volume = 0.2f;
                    tireSmoke.Play();
                } else {
                    squealSound.volume = 0f;
                    tireSmoke.Stop();
                }

                // Smoothly transition engine pitch to target pitch
                engineSound.pitch = Mathf.Lerp(engineSound.pitch, targetPitch, Time.deltaTime);
            }
        }
    }
}
