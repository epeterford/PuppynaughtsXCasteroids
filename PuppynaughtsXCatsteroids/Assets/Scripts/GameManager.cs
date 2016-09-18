using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Image dogScore; 
    public Image catScore; 
    public Text roundTimer; 
    public float timeLeft; 
    public GameObject scoreUI;
    public Image dogCoolDown;
    public Image catCoolDown; 
    public bool catNeedsCoolDown = false;
    public bool dogNeedsCoolDown = false;
    public float commonGoal;
    static bool commonGoalMet = false;
    public bool gameStarted = false;
    public Text startTimer;
    public float timeToStart; 
	// Use this for initialization
	void Start () 
    {
        
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CommonGoal", 0);
        dogScore.fillAmount = 0f;
        catScore.fillAmount = 0f;
        timeLeft = 60;
        commonGoal = 100;
        timeToStart = 5;
        startTimer.text = timeToStart.ToString();
        roundTimer.gameObject.SetActive(false);
        StartGameTimer();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(gameStarted)
        {
            UpdateCurrentScore();

            UpdateRoundTime();

            CheckIfCommonGoalMet();

            if(catNeedsCoolDown)
            {
                catCoolDown.fillAmount +=1.0f/3 * Time.deltaTime;
                if(catCoolDown.fillAmount>=1)
                {
                    catNeedsCoolDown = false;
                }
            }
            if(dogNeedsCoolDown)
            {
                dogCoolDown.fillAmount +=1.0f/3 * Time.deltaTime;
                if(dogCoolDown.fillAmount>=1)
                {
                    dogNeedsCoolDown = false;
                }
            }
        }
        else
        {
            StartGameTimer();
        }
	}
    void StartGameTimer()
    {

        //Subtract Time
        timeToStart -= Time.deltaTime;
        startTimer.text = Mathf.Round(timeToStart).ToString();

        if(timeToStart<0)
        {
            //Start Game();
            Destroy(startTimer.gameObject);
            gameStarted = true;
            roundTimer.gameObject.SetActive(true);
        }

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
            PlayerPrefs.SetInt("CommonGoal", 1); 
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

    public void CatCoolDown()
    {
        catCoolDown.fillAmount +=1.0f/3 * Time.deltaTime;
    }
    public void DogCoolDown()
    {
        dogCoolDown.fillAmount +=1.0f/3 * Time.deltaTime;
    }
    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
        
}
