using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

public class ScoreText : MonoBehaviour
{
    public Text PlayerName;
    public Text score;
    public GameObject Panel;
    public GameObject Score;


    public void ChangeName(string n)
    {
        PlayerName.text = n;
    }
    public void ChangeScore(int s)
    {
        score.text = s.ToString();
    }
    public void AddScore()
    {
        int points = Convert.ToInt32(score.text);
        points++;
        score.text = points.ToString();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Panel.SetActive(true);
            Score.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Panel.SetActive(false);
            Score.SetActive(false);
        }
    }
}
