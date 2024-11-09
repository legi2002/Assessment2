using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public float booster = 2f;
    public float duration = 5f;
    public bool boosterActive = false;
    public float rotationSpeed = 90f;

    private Renderer boostRender;
    private Collider boostCollider;
    private AudioSource boostSound;

    private void Start()
    {
        boostRender = GetComponent<Renderer>();
        boostCollider = GetComponent<Collider>();
        boostSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        CarControl carCont = other.GetComponent<CarControl>();

        if(carCont != null && !boosterActive)
        {
            StartCoroutine(boostBaby(carCont));
            if(boostSound != null)
            {
                boostSound.Play();
            }
        }
    }

    private IEnumerator boostBaby(CarControl localCC)
    {
        boosterActive = true;

        float OMT = localCC.motorTorque; //ORiginal Motor torque
        localCC.motorTorque *= booster; // Apply the boost multiplier

        boostRender.enabled = false;
        boostCollider.enabled = false;

        yield return new WaitForSeconds(duration); // Wait for the boost duration

        localCC.motorTorque = OMT; // Reset to the original motor torque
        boostRender.enabled = true;
        boostCollider.enabled = true;
        boosterActive = false;
    }
}
