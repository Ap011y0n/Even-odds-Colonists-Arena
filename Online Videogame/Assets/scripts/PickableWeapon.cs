using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PickableWeapon : MonoBehaviourPunCallbacks, IPunObservable
{

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(destroy);

        }
        else
        {
            // Network player, receive data
            this.destroy = (bool)stream.ReceiveNext();


        }
    }

    public bool destroy = false;
    public string weaponName;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {
            destroyWeapon();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && photonView.IsMine)
        {
            if (other.gameObject.GetComponent<PlayerManager>().deleteFloorGun)
            {
                destroy = true;

            }
        }
    }

    public void destroyWeapon()
    {
        Debug.Log("destroyin gun");

        PhotonNetwork.Destroy(photonView);
    }
}
