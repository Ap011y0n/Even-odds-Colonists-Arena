using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class scoreSender : MonoBehaviourPunCallbacks, IPunObservable
{

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("Receiving info");

        if (stream.IsWriting)
        {
            stream.SendNext(scores);

        }
        else
        {
            this.scores = (Dictionary<string, int>)stream.ReceiveNext();
            ScoreCounter.instance.scores = scores;
            foreach (KeyValuePair<string, int> entry in scores)
            {
                Debug.Log(entry.Key + entry.Value.ToString());
                ScoreCounter.instance.requestScore(entry.Key);
                ScoreCounter.instance.ChangeScores();
            }
        }
    }

    Dictionary<string, int> scores = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            scores = ScoreCounter.instance.scores;

            //foreach (KeyValuePair<string, int> entry in scores)
            //{
            //    Debug.Log(entry.Key + entry.Value.ToString());

            //}
        }

    }
}
