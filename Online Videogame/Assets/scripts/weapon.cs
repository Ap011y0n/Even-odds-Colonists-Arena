using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class weapon : MonoBehaviour
{
    public GameObject projectile;
    public float shotCD = 1;
    float lastShot = 0;
    public string weaponName;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //if(gameObject.transform.parent != null)
        //transform.localPosition = offset;

    }

    // Update is called once per frame
    void Update()
    {
      

        lastShot += Time.deltaTime;

      
            
    }


    public void fire()
    {
        if(lastShot >= shotCD)
        {
            lastShot = 0;
            Instantiate(projectile, transform.position, transform.rotation);
            projectile.GetComponent<bullet>().setParent(player.GetComponent<PlayerManager>().photonView.Owner.NickName);
        }
    }

}
