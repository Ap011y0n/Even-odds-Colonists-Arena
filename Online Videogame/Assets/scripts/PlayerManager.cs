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
            stream.SendNext(pickupGun);
            stream.SendNext(changeGun);
            stream.SendNext(activegun.GetComponent<weapon>().weaponName);
            stream.SendNext(deleteFloorGun);
            stream.SendNext(swap1);
            stream.SendNext(swap2);


        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
            this.pickupGun = (bool)stream.ReceiveNext();
            this.changeGun = (bool)stream.ReceiveNext();
            this.currentGunName = (string)stream.ReceiveNext();
            this.deleteFloorGun = (bool)stream.ReceiveNext();
            this.swap1 = (bool)stream.ReceiveNext();
            this.swap2 = (bool)stream.ReceiveNext();
        }
    }


    #endregion

    #region Private Fields

    [Tooltip("The Beams GameObject to control")]
    [SerializeField]
    private GameObject beams;
    //True, when the user is firing
    bool IsFiring;
    bool pickupGun;
    bool changeGun;
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
    public GameObject inactivegun = null;
    private GameObject[] weaponSlots = new GameObject[2];
    public bool swap1 = false;
    public bool swap2 = false;

    public string currentGunName = "pistol";
    private string foundWeapon;
    public List<GameObject> weapons = new List<GameObject>();


    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;
    public GameObject ScoreUiPrefab;


    private PlayerUI myUI;

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
        if (PlayerUiPrefab != null && photonView.IsMine)
        {
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            myUI = _uiGo.GetComponent<PlayerUI>();
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }
        ScoreCounter.instance.requestScore(photonView.Owner.NickName);

        weaponSlots[0] = weapons[0];
        weaponSlots[1] = null;
    

    }

    void Update()
    {
        if(myUI == null && !photonView.IsMine)
            ScoreCounter.instance.requestScore(photonView.Owner.NickName);

        if (myUI == null && photonView.IsMine)
        {
            ScoreCounter.instance.requestScore(photonView.Owner.NickName);
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            myUI = _uiGo.GetComponent<PlayerUI>();
        }

        if (photonView.IsMine)
            GameManager.Instance.secCameraObj.SetActive(false);

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
                pickGun();
            }
            // beams.SetActive(IsFiring);


        }
        if (pickupGun || currentGunName != activegun.GetComponent<weapon>().weaponName)
        {

            pickGun();

        }
        if (swap1)
            swaptoSlot1();
        if (swap2)
            swaptoSlot2();
           
       
    }
    void pickGun()
    {

            Debug.Log("finding new gun");

        if (!photonView.IsMine)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].GetComponent<weapon>().weaponName == currentGunName)
                {
                    if (weaponSlots[1] == null)
                        weaponSlots[1] = weapons[i];
                    else if ((weaponSlots[1].GetComponent<weapon>().weaponName == activegun.GetComponent<weapon>().weaponName &&
                        weaponSlots[1].GetComponent<weapon>().weaponName != currentGunName))
                        weaponSlots[1] = weapons[i];
                    else
                        weaponSlots[0] = weapons[i];
                    

                    pickupGun = false;
                    activegun.SetActive(false);
                    inactivegun = activegun;
                    activegun = weapons[i];
                    activegun.SetActive(true);
                }

            }

        }
        else
            pickupGun = false;
    }
   
    void swaptoSlot1 ()
    {
        inactivegun = activegun;
        activegun = weaponSlots[0];
        inactivegun.SetActive(false);
        activegun.SetActive(true);
        swap1 = false;
        currentGunName = activegun.GetComponent<weapon>().weaponName;

    }
    void swaptoSlot2()
    {
        inactivegun = activegun;
        activegun = weaponSlots[1];
        inactivegun.SetActive(false);
        activegun.SetActive(true);
        currentGunName = activegun.GetComponent<weapon>().weaponName;
        swap2 = false;
    }

    void OnTriggerEnter(Collider other)
    {
        

        //if (!photonView.IsMine)
        //{
        //    return;
        //}
        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (other.CompareTag("bullet"))
        {
            Health -= 10f;
            Debug.Log("ouchies" + Health.ToString());

            if (PhotonNetwork.IsMasterClient && Health <= 0f)
            {
                string killer = other.GetComponent<bullet>().returnParent();
                Debug.LogWarning(killer);
                ScoreCounter.instance.AddScore(killer);

            }
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


                for (int i = 0; i < weapons.Count; i++)
                {
                    Debug.Log(weapons[i].GetComponent<weapon>().weaponName);

                    if (weapons[i].GetComponent<weapon>().weaponName == currentGunName)
                    {
                        if (weaponSlots[1] == null)
                            weaponSlots[1] = weapons[i];
                        else if((weaponSlots[1].GetComponent<weapon>().weaponName == activegun.GetComponent<weapon>().weaponName &&
                            weaponSlots[1].GetComponent<weapon>().weaponName != currentGunName))
                            weaponSlots[1] = weapons[i];
                        else
                            weaponSlots[0] = weapons[i];
                        pickupGun = true;
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
        if(Input.GetKeyDown(KeyCode.Alpha1) && weaponSlots[0] != null)
        {
            swap1 = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponSlots[1] != null)
        {
            swap2 = true;
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
