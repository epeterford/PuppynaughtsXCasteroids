﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Image dogScore; 
    public Image catScore; 
    public Text roundTimer; 
    public float timeLeft; 
    public GameObject scoreUI;
    public float commonGoal;
    bool commonGoalMet = false;
	// Use this for initialization
	void Start () 
    {
        dogScore.fillAmount = 0f;
        catScore.fillAmount = 0f;
        timeLeft = 60;
        commonGoal = 100;
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateCurrentScore();

        UpdateRoundTime();

        CheckIfCommonGoalMet();

	}

    void UpdateCurrentScore()
    {
        if(dogScore.fillAmount>catScore.fillAmount)
        {
            dogScore.transform.SetAsLastSibling();
        }
        else
        {
            catScore.transform.SetAsLastSibling();
        }

        roundTimer.transform.SetAsLastSibling();

    }

    void CheckIfCommonGoalMet()
    {
        if((dogScore.fillAmount*100) + (catScore.fillAmount*100) >=100)
        {
            commonGoalMet = true; 
        }
    }

    public void DogScores(float points)
    {
        dogScore.fillAmount+=points;
        Debug.Log("dog given: " + points);
    }

    public void CatScores(float points)
    {
        catScore.fillAmount+=points;
    }
    void UpdateRoundTime()
    {
        //Subtract Time
        timeLeft -= Time.deltaTime;
        roundTimer.text = Mathf.Round(timeLeft).ToString();

        if(timeLeft<0)
        {
            GameOver();
        }
        else if ( timeLeft>10)
        {
            roundTimer.text = Mathf.Round(timeLeft).ToString(); 
        }
        else
        {
            roundTimer.text = timeLeft.ToString("F2");
        }

    }

    void GameOver()
    {
        //SceneManager.LoadScene("GameOver");
    }

    public void SpawnPointsUI(Vector3 pos)
    {
        /*Vector3 scorePos = Camera.main.WorldToScreenPoint(pos);
        scorePos.x = scorePos.x / Screen.width;
        scorePos.y = scorePos.y / Screen.height;
        Instantiate(scoreUI, scorePos, Quaternion.identity);*/
    }
}
