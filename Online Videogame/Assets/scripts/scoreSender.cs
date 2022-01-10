using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class scoreSender : MonoBehaviourPunCallbacks
{

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("Receiving info");

       
    }

    Dictionary<string, int> scores = new Dictionary<string, int>();

    // Start is called before the first frame update
    private void Awake()
    {
        
       
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && ScoreCounter.instance.send)
        {
            ScoreCounter.instance.send = false;
            scores = ScoreCounter.instance.scores;
            photonView.RPC("updateValues", RpcTarget.AllBuffered, scores);
        }

    }

    [PunRPC]
    void updateValues(Dictionary<string, int> scores)
    {
        Debug.Log("Hola");
        this.scores = scores;
        ScoreCounter.instance.scores = scores;
        ScoreCounter.instance.ChangeScores();

        foreach (KeyValuePair<string, int> entry in scores)
        {
            Debug.Log(entry.Key + entry.Value.ToString());
            
        }
    }
}
