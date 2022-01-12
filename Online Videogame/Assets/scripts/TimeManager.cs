using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public GameObject UI_timer;
    public int maxTime;
    public int update;
    private float timer;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);

    }
    void Start()
    {
        if (UI_timer == null)
            UI_timer = GameObject.Find("Timer");
        UI_timer.GetComponent<TMPro.TextMeshProUGUI>().text = maxTime.ToString();
        timer = maxTime;
        update = maxTime - 1;
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {

        if(timer > 0)
        timer -= Time.deltaTime;
        if(timer <= update)
        {
            if (UI_timer == null)
                UI_timer = GameObject.Find("Timer");
            string minutes = (update / 60).ToString();
            string seconds = (update % 60).ToString();
            if ((update % 60) < 10)
            {
                UI_timer.GetComponent<TMPro.TextMeshProUGUI>().text = minutes + ":0" + seconds;
            }
            else
                UI_timer.GetComponent<TMPro.TextMeshProUGUI>().text = minutes + ":" + seconds;

            update = (int)timer;
        }
        if (timer <= 0)
        {
            GameManager.Instance.endMatch();
        }
    }

    public float getTime()
    {
        return timer;
    }
    public void setTime(float t)
    {
        timer = t;
    }
}
