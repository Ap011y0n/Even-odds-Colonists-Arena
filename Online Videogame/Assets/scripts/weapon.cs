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
    public bool unlimitedAmmo = true;
    [HideInInspector]
    public int ammo = 20;
    public int maxammo = 20;
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
        if(lastShot >= shotCD && (ammo > 0 || unlimitedAmmo))
        {
            lastShot = 0;
            Instantiate(projectile, transform.position, transform.rotation);
            projectile.GetComponent<bullet>().setParent(player.GetComponent<PlayerManager>().photonView.Owner.NickName);
            if (!unlimitedAmmo)
                ammo--;
        }
    }
    
    public int returnAmmo()
    {
        return ammo;
    }
    public void setMaxAmmo()
    {
        ammo = maxammo;
    }

}
