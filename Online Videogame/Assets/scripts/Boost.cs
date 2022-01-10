using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Boost : MonoBehaviourPunCallbacks, IPunObservable
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
    public string boostName;

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
            destroyBoost();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && photonView.IsMine)
        {
            destroy = true;
        }
    }

    public void destroyBoost()
    {
        Debug.Log("Destroying Boost");

        PhotonNetwork.Destroy(photonView);
    }
}
