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
    private GameObject GunText;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (destroy && photonView.IsMine)
        {
            destroyWeapon();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (other.gameObject.GetComponent<PlayerManager>().deleteFloorGun)
            {
                destroy = true;
                other.gameObject.GetComponent<PlayerManager>().deleteFloorGun = false;
            }
            else if(other.gameObject.GetComponent<PlayerManager>().weaponSlots[0].GetComponent<weapon>().weaponName == weaponName)
            {
                destroy = true;
                Debug.Log("Reload 1");
                other.gameObject.GetComponent<PlayerManager>().weaponSlots[0].GetComponent<weapon>().setMaxAmmo();
            }
            else if (other.gameObject.GetComponent<PlayerManager>().weaponSlots[1] != null &&
                other.gameObject.GetComponent<PlayerManager>().weaponSlots[1].GetComponent<weapon>().weaponName == weaponName)
            {
                Debug.Log("Reload 2");
                destroy = true;
                other.gameObject.GetComponent<PlayerManager>().weaponSlots[1].GetComponent<weapon>().setMaxAmmo();
            }
        }
    }

    public void destroyWeapon()
    {
        Debug.Log("destroyin gun");

        PhotonNetwork.Destroy(photonView);
        if(GunText != null)
        {
            GunText.SetActive(false);
        }
    }

    public void SetGunText(GameObject t)
    {
        GunText = t;
    }
}
