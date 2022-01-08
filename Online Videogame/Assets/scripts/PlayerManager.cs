using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

using System.Collections;
using System.Collections.Generic;



public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{

    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(IsFiring);
            stream.SendNext(Health);
            stream.SendNext(ChangeGun);
            stream.SendNext(activegun.GetComponent<weapon>().weaponName);
            stream.SendNext(deleteFloorGun);

        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
            this.ChangeGun = (bool)stream.ReceiveNext();
            this.currentGunName = (string)stream.ReceiveNext();
            this.deleteFloorGun = (bool)stream.ReceiveNext();
            Debug.LogWarning(currentGunName);
        }
    }


    #endregion

    #region Private Fields

    [Tooltip("The Beams GameObject to control")]
    [SerializeField]
    private GameObject beams;
    //True, when the user is firing
    bool IsFiring;
    bool ChangeGun;
    [HideInInspector]
    public bool deleteFloorGun = false;

    [Tooltip("The current Health of our player")]
    public float Health = 100f;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    #endregion
    // public GameObject bullet;
    //public GameObject gun;
    public GameObject camera;
    public GameObject activegun;
    private string currentGunName = "";
    private string foundWeapon;
    public List<GameObject> weapons = new List<GameObject>();

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        if (beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else
        {
            beams.SetActive(false);
        }

        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //Photon.Pun.Demo.PunBasics.CameraWork _cameraWork = this.gameObject.GetComponent<Photon.Pun.Demo.PunBasics.CameraWork>();


        //if (_cameraWork != null)
        //{
        //    if (photonView.IsMine)
        //    {
        //        _cameraWork.OnStartFollowing();
        //    }
        //}
        //else
        //{
        //    Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        //}
    }
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {

        if (photonView.IsMine)
        {
            ProcessInputs();
            if (Health <= 0f)
            {
                // GameManager.Instance.LeaveRoom();
                die();
            }
        }
        // trigger Beams active state
        if (IsFiring)
        {
            if(activegun != null)
            activegun.GetComponent<weapon>().fire();
            else
            {
                Debug.Log("Can't find gun");
                changeGun();
            }
            // beams.SetActive(IsFiring);


        }
        if (ChangeGun || currentGunName != activegun.GetComponent<weapon>().weaponName)
        {

            changeGun();

        }
        
           
       
    }
    void changeGun()
    {

            Debug.Log("finding new gun");

        if (!photonView.IsMine)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].GetComponent<weapon>().weaponName == currentGunName)
                {
                    ChangeGun = false;
                    activegun.SetActive(false);
                    activegun = weapons[i];
                    activegun.SetActive(true);
                }

            }

        }
        else
            ChangeGun = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (other.CompareTag("bullet"))
        {
            Health -= 10f;
        }


    }

    void OnTriggerStay(Collider other)
    {
        // we dont' do anything if we are not the local player.
        if (!photonView.IsMine)
        {
            return;
        }

      
        if(other.CompareTag("pickableWeapon") && photonView.IsMine)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                currentGunName = other.gameObject.GetComponent<PickableWeapon>().weaponName;
                Debug.Log(currentGunName);


                for (int i = 0; i < weapons.Count; i++)
                {
                    Debug.Log(weapons[i].GetComponent<weapon>().weaponName);

                    if (weapons[i].GetComponent<weapon>().weaponName == currentGunName)
                    {
                        ChangeGun = true;
                        activegun.SetActive(false);
                        activegun = weapons[i];
                        activegun.SetActive(true);
                    }

                }

                deleteFloorGun = true;
            }
        }
 
    }
    #endregion

    #region Custom

    /// <summary>
    /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
    /// </summary>
    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!IsFiring)
            {
                IsFiring = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (IsFiring)
            {
                IsFiring = false;
            }
        }
    }

    void die()
    {

        if (photonView.IsMine)
        {
            GameManager.Instance.AddRespawn();
            GameManager.Instance.secCameraObj.SetActive(true);
            //GameManager manager = GameManager.Instance;
            //manager.AddRespawn();
            PlayerManager.LocalPlayerInstance = null;
            PhotonNetwork.Destroy(photonView);
        }
    }
    #endregion
}
