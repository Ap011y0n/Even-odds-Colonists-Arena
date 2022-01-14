using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HitmarkerScript : MonoBehaviour
{
    public float maxLifeTime = 0.4f;
    private float lifeTime = 0f;
    bool displayed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(displayed)
        {
            lifeTime += Time.deltaTime;
            if (lifeTime > maxLifeTime)
            {
                displayed = false;
                GetComponent<Image>().enabled = displayed;
            }
        }
      
    }
    public void Display()
    {
        displayed = true;
        lifeTime = 0;
        GetComponent<Image>().enabled = displayed;
    }
}
