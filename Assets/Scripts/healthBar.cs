using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public Slider healthSlider;
    public GameOverScript overScreen;
    public float damageAmount = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthSlider != null)
        {
            healthSlider.value = health;
        }
        if(health <= 0)
        {
            GameOver();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        TakeDamage(damageAmount);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if(healthSlider != null)
        {
            healthSlider.value = health;
        }
    }

    public void GameOver()
    {
        overScreen.Setup();
    }
}
