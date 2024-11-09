using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Refill : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI refillAvailableText;
    
    public float addHealth = 40f;
    public float duration = 60f;
    public bool isAvailable = true;
    public float rotationSpeed = 90f;

    private Renderer refillRender;
    private Collider refillCollider;
    private AudioSource refillSound;

    private void Start()
    {
        refillRender = GetComponent<Renderer>();
        refillCollider = GetComponent<Collider>();
        refillSound = GetComponent<AudioSource>();

    }
    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        healthBar healthBar = other.GetComponent<healthBar>();

        if (healthBar != null && isAvailable)
        {
            healthBar.TakeDamage(-addHealth);
            StartCoroutine(Cooldown());

            if (refillAvailableText != null)
            {
                refillSound.Play();
                refillAvailableText.text = "N/A";
            }
        }
    }
    private IEnumerator Cooldown()
    {
        isAvailable = false;
        refillRender.enabled = false;
        refillCollider.enabled = false;

        yield return new WaitForSeconds(duration);

        isAvailable = true;
        refillCollider.enabled = true;
        refillRender.enabled = true;

        if (refillAvailableText != null)
        {
            refillAvailableText.text = "Available";
        }
    }
}
