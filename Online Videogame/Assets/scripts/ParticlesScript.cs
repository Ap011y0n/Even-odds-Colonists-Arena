using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float LifeTime = 2;
    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
