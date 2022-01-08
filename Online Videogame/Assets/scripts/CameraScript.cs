using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraScript : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public AudioListener listener;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        
            camera.enabled = true;
            listener.enabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
