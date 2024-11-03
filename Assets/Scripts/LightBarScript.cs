using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightBarScript : MonoBehaviour
{

    public Material red;
    public Material blue;
    public Object leftLight;
    public Object rightLight;
    private bool changeLight;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PoliceLights());
    }

    IEnumerator PoliceLights() {
        while (true) {
            changeLight = !changeLight;
            leftLight.GetComponent<MeshRenderer>().material = changeLight ? red : blue;
            rightLight.GetComponent<MeshRenderer>().material = changeLight ? blue : red;
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
