using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreCounter : MonoBehaviour
{
   
    public static ScoreCounter instance;
    public struct request
    {
       public string name;
      
    }
    List<GameObject> childs = new List<GameObject>();
    public Dictionary<string, int> scores = new Dictionary<string, int>();
    public GameObject scoreUI;
    public GameObject UiPlayerPrefab;
    List<request> requests = new List<request>();
    public float height = 20;
    public float width = 0;
    void Awake()
    {

        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);

    }

    // Start is called before the first frame update
    void Start()
    {
        scoreUI = GameObject.Find("ScorePanel");
        foreach (KeyValuePair<string, int> entry in scores)
        {
            requestScore(entry.Key);
        }
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if(scoreUI == null)
        {
            scoreUI = GameObject.Find("ScorePanel");
            childs.Clear();
        }
        else
        {
            if(requests.Count > 0)
            {
                foreach (request r in requests)
                {
                    Addrequest(r);
                }
                requests.Clear();
            }
    
        }

    }

    public void Addrequest(request r)
    {
        if (GameObject.Find(r.name) != null)
        {
            return;
        }

        if (!scores.ContainsKey(r.name))
            scores.Add(r.name, 0);

        GameObject obj = Instantiate(UiPlayerPrefab);
        obj.transform.SetParent(scoreUI.transform);
        obj.transform.localPosition = new Vector3(width * childs.Count, height * childs.Count, 0);
        obj.name = r.name;
        obj.GetComponent<ScoreText>().ChangeName(r.name);
        obj.GetComponent<ScoreText>().ChangeScore(scores[r.name]);
        childs.Add(obj);
    }
    public void ChangeScores()
    {
        foreach (KeyValuePair<string, int> entry in scores)
        {
            Debug.Log("Change scores");
            for(int i = 0; i < childs.Count; i++)
            {
                if(childs[i].name == entry.Key)
                childs[i].GetComponent<ScoreText>().ChangeScore(entry.Value);
            }

        }
    }
    public void requestScore(string Name)
    {
        if (GameObject.Find(Name) != null)
        {
            return;
        }
        request r = new request();
        r.name = Name;
        requests.Add(r);
       
    }
    //public void requestScoreWithValue(string Name, int value)
    //{
    //    if (GameObject.Find(Name) != null)
    //    {
    //        return;
    //    }
    //    request r = new request();
    //    r.name = Name;
    //    r.value = value;
    //    requests.Add(r);

    //}

    public void AddScore(string name)
    {
       //Debug.LogError("Adding Score for " + name);
        for (int i = 0; i < childs.Count; i++)
        {
           // Debug.Log(childs[i].name + "  " + name);
            if (childs[i].name == name)
            {
                childs[i].GetComponent<ScoreText>().AddScore();
                scores[name] += 1; 
            }
        }
    }
}
