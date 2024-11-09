using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public float Gained_XP = 0.0f;

    public void gainXP(float XP)
    {
        Gained_XP +=XP;
    }
}
