using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class weaponSpawn : MonoBehaviourPunCallbacks
{

    // public static GameObject SpawnInstance;
    public float spawnTime;
    float counter = 0;
    private GameObject spawnGun = null;
    bool spawned = false;
    public static weaponSpawn instance;
    public GameObject gun;
    string gunName = "PickableRifle";


    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);

    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }
    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient && !spawned && spawnGun == null)
        counter += Time.deltaTime;
        if(counter >= spawnTime)
        {
            spawned = true;
            
        }
        if(spawned)
        {
            spawned = false;
            counter = 0;
            if (gun != null)
                spawnGun = PhotonNetwork.Instantiate(gun.name, transform.position, Quaternion.identity, 0);
            else
                spawnGun = PhotonNetwork.Instantiate(gunName, transform.position, Quaternion.identity, 0);

        }

    }
}
