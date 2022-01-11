using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerLook : MonoBehaviourPunCallbacks
{
    
    public float sensibility = 100;
    public float upRot;
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameEnded || (photonView.IsMine == false && PhotonNetwork.IsConnected == true))
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X"); //* sensibility * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y"); //* sensibility * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);
            upRot -= mouseY;
            upRot = Mathf.Clamp(upRot, -90, 90);
            camera.transform.localRotation = Quaternion.Euler(upRot, 0, 0);
        
    }
}
