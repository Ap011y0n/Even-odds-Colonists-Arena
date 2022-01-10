using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class timeSender : MonoBehaviourPunCallbacks
{

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("Receiving info");

       
    }

    float time;



    void Start()
    {
        if(photonView.IsMine)
        {
            time = TimeManager.instance.getTime();
            photonView.RPC("updatetime", RpcTarget.AllBuffered, time);

        }
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    [PunRPC]
    void updatetime(float t)
    {
        time = t;
        TimeManager.instance.setTime(time);
    }
}
