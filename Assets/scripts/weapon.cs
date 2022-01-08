using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class weapon : MonoBehaviourPunCallbacks, IPunObservable
{
    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(HasParent);
            stream.SendNext(destroy);
            
        }
        else
        {
            // Network player, receive data
            this.HasParent = (bool)stream.ReceiveNext();
            this.destroy = (bool)stream.ReceiveNext();


        }
    }


    #endregion
    public GameObject bullet;
    public float shotCD = 1;
    float lastShot = 0;
    public Vector3 offset;
    public bool HasParent = true;
    public string weaponName;
    public bool destroy = false;
    // Start is called before the first frame update
    void Start()
    {
        //if(gameObject.transform.parent != null)
        //transform.localPosition = offset;

    }

    // Update is called once per frame
    void Update()
    {
        if (HasParent && transform.parent == null)
        {
            GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
            float distance = 1;
            for (int i = 0; i < cameras.Length; i++)
            {
                float newDistance = Vector3.Distance(transform.position, cameras[i].transform.position);
                if (newDistance <= distance)
                {
                    distance = newDistance;
                    transform.SetParent(cameras[i].transform, false);
                    transform.localPosition = offset;
                }
            }
        }

        lastShot += Time.deltaTime;

        if(destroy)
        {
            destroyWeapon();
        }

            
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && photonView.IsMine)
        {
            if (other.gameObject.GetComponent<PlayerManager>().deleteFloorGun)
            {
                            destroy = true;

            }
        }
    }
    public void fire()
    {
        if(lastShot >= shotCD)
        {
            lastShot = 0;
            Instantiate(bullet, transform.position, transform.rotation);

        }
    }
    public void destroyWeapon()
    {
        Debug.Log("destroyin gun");

        PhotonNetwork.Destroy(photonView);
    }
}
