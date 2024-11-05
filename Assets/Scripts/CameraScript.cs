using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform car;
    public float speed;
    public float damping;
    Vector3 localRotation;

    void Update() {
        transform.position = car.position;

        localRotation.x += Input.GetAxis("Mouse X") * speed;
        localRotation.y -= Input.GetAxis("Mouse Y") * speed;

        localRotation.y = Mathf.Clamp(localRotation.y, -10f, 70f);

        Quaternion rotation = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
        // transform.rotation = rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
}
