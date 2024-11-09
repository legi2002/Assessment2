using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class scoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    public XPBar score;

    private void Start()
    {
        // Optionally, set an initial value
        score = FindObjectOfType<XPBar>();
        
        UpdateScoreText(); 
    }

    private void Update()
    {
        UpdateScoreText(); // Update every frame to reflect current XP
    }

    private void UpdateScoreText()
    {
        if (score != null)
        {
            scoreText.text = $"{score.Gained_XP:0}";
        }
    }
}
