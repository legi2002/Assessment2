using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSpawner : MonoBehaviour
{
    public GameObject police;
    public int X;
    public int Z;
    public int count = 0;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        while(count != 5)
        {
            Instantiate(police,new Vector3(X,1,Z), Quaternion.identity);
            yield return new WaitForSeconds(45);
            count =+1  ;
        }        
    }
}
