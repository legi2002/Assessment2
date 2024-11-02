using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public Image bar;
    public float damageAmount = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = Mathf.Clamp(health/maxHealth, 0 ,1);
    }
    void onCollisionEnter(Collider collision)
    {
        TakeDamage(damageAmount);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log(health);
    }
}
