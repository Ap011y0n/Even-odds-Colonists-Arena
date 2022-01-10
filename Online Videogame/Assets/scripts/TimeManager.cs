using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public Text UI_timer;
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
            UI_timer = GameObject.Find("Timer").GetComponent<Text>();
        UI_timer.text = maxTime.ToString();
        timer = maxTime;
        update = maxTime - 1;
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {

        
        timer -= Time.deltaTime;
        if(timer <= update)
        {
            if (UI_timer == null)
                UI_timer = GameObject.Find("Timer").GetComponent<Text>();
            UI_timer.text = update.ToString();
            update = (int)timer;
        }
        if (timer <= 0)
        {
            GameManager.Instance.LeaveRoom();
        }
    }

    public float getTime()
    {
        return timer;
    }
    public void setTime(float t)
    {
        Debug.Log("Hey");
        timer = t;
    }
}
