using System.Collections;
using UnityEngine;

public class XPBlock : MonoBehaviour
{
    public float xp_amount = 10f;
    public float cooldownseconds = 60f;
    private bool cooldownPhase = false;
    public float rotationSpeed = 90f;


    private Renderer XPRender;
    private Collider XPCollider;
    private AudioSource boostSound;

    private void Start()
    {
        XPRender = GetComponent<Renderer>();
        XPCollider = GetComponent<Collider>();
        boostSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        XPBar xp = other.GetComponent<XPBar>();

        if (xp != null && !cooldownPhase)
        {
            xp.gainXP(xp_amount);
            StartCoroutine(Cooldown());
            if(boostSound != null)
            {
                boostSound.Play();
            }
        }
    }

    private IEnumerator Cooldown()
    {
        cooldownPhase = true;

        XPCollider.enabled = false;
        XPRender.enabled = false;

        yield return new WaitForSeconds(cooldownseconds); 

        XPCollider.enabled = true;
        XPRender.enabled = true;
        cooldownPhase = false;
    }
}
