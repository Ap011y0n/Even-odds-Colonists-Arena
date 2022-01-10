using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeObjs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] permanentObjs = GameObject.FindGameObjectsWithTag("permanent");
       
        foreach (GameObject go in permanentObjs)
        {
            Destroy(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
