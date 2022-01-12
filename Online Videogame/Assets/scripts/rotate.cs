using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called before the first frame update
    float rotationsPerMinute = 10.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);
    }
}
