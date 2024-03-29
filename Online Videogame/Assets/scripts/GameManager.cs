using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public GameObject weaponPrefab;

    public GameObject secCameraObj;
    public GameObject timeSenderPrefab;

    public static GameManager Instance;
    private bool respawn = false;
    public int respawnTime = 5;
    private float respawnTimer = 0;

    public GameObject[] endGameUI;
    public bool GameEnded = false;
    public Vector3[] startingPositions;
    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    /// 



    #region Photon Callbacks

    void Start()
    {
        Instance = this;
        
        instantiatePlayer();
         if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(this.timeSenderPrefab.name, new Vector3(0f, 10f, 0f), Quaternion.identity, 0);
        }

    }

    void instantiatePlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                secCameraObj.SetActive(false);
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                Vector3 SpawnPos = startingPositions[Random.Range(0, startingPositions.Length - 1)];
                PhotonNetwork.Instantiate(this.playerPrefab.name, SpawnPos, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }

        }
    }

    private void Update()
    {
       
        if (respawn)
        {
            respawnTimer += Time.deltaTime;
            if(respawnTimer>=respawnTime)
            {
                respawn = false;
                instantiatePlayer();
            }
        }
    }
    public void AddRespawn()
    {

        respawnTimer = 0;
        respawn = true;
    }
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        // PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Room for 1");
    }
    #endregion

    public void endMatch()
    {
        foreach (GameObject obj in endGameUI)
            obj.SetActive(true);

        GameEnded = true;
        Cursor.lockState = CursorLockMode.None;

    }
}

