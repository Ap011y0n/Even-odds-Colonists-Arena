using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class boostSpawn : MonoBehaviourPunCallbacks
{

    // public static GameObject SpawnInstance;
    public float spawnTime;
    float counter = 0;
    private GameObject spawnBoost = null;
    bool spawned = false;
    public GameObject boost;
    string boostName = "SpeedBoost";


    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] permanentOBJ = GameObject.FindGameObjectsWithTag("permanent"); 
        int duplicated = 0;
        for (int i = 0; i < permanentOBJ.Length; i++)
        {
            if (permanentOBJ[i].name == gameObject.name)
                duplicated++;
        }
        if (duplicated > 1)
            DestroyImmediate(gameObject);


    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient && !spawned && spawnBoost == null)
        counter += Time.deltaTime;
        if(counter >= spawnTime)
        {
            spawned = true;
            
        }
        if(spawned)
        {
            spawned = false;
            counter = 0;
            if(boost != null)
                spawnBoost = PhotonNetwork.Instantiate(boost.name, transform.position, Quaternion.identity, 0);
            else
                spawnBoost = PhotonNetwork.Instantiate(boostName, transform.position, Quaternion.identity, 0);

            Debug.Log("Boost spawned");
        }

    }
}
