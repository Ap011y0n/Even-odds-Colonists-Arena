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
            stream.SendNext(activegun.name);
            stream.SendNext(deleteFloorGun);

            if (ChangeGun)
                ChangeGun = false;
            newGunName = "";
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
            this.ChangeGun = (bool)stream.ReceiveNext();
            this.newGunName = (string)stream.ReceiveNext();
            this.deleteFloorGun = (bool)stream.ReceiveNext();
            Debug.LogWarning(newGunName);
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
    private string newGunName = "";
    private string foundWeapon;
    public List<string> weaponNames = new List<string>(); 
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
        if (ChangeGun)
        {

            changeGun();
        }
        
           
       
    }
    void changeGun()
    {
            Debug.Log("finding new gun");

            if (!photonView.IsMine)
            {
                Debug.LogWarning("CHANGEGUN");
            if(activegun != null)
                activegun.SetActive(false);

                GameObject[] guns = GameObject.FindGameObjectsWithTag("gun");

                float distance = 100;
                for (int i = 0; i < guns.Length; i++)
                {
                    Debug.Log(newGunName);

                    Debug.Log(guns[i].name);
                    Debug.Log(guns.Length);

                    float newDistance = Vector3.Distance(transform.position, guns[i].transform.position);
                    if (newDistance <= distance && guns[i].name == newGunName)
                    {
                        Debug.Log("Final gun" + guns[i].name);

                        ChangeGun = false;
                        distance = newDistance;
                        activegun = guns[i];
                        activegun.GetComponent<Collider>().enabled = false;
                    }
                }



            }

        
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

      
        if(other.CompareTag("gun") && photonView.IsMine)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeGun = true;
                newGunName = other.gameObject.GetComponent<weapon>().weaponName;
                Debug.Log(newGunName);
                GameObject gun = PhotonNetwork.Instantiate(other.gameObject.GetComponent<weapon>().weaponName, 
                    camera.transform.position/* + other.gameObject.GetComponent<weapon>().offset*/, 
                    Quaternion.identity, 0);

                activegun.SetActive(false);

                activegun = gun;
                activegun.GetComponent<Collider>().enabled = false;
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
