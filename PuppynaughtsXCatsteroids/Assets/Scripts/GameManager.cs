/// <summary>
/// Game Manager. 
/// Keeps track of current player scores, rules, and round time.  
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    //UI
	public Image dogScore; 
	public Image catScore; 
    public Text roundTimer; 
    public Text startTimer;
   
    float dogScoreValue = 0f;
    float catScoreValue = 0f;

    float commonGoal = 100; 

    float timeLeft = 90; // time left in round
    float countdownTime = 5; 
	
	public bool gameStarted = false;

	// Use this for initialization
	void Start () 
	{
		PlayerPrefs.DeleteAll(); 
      
		roundTimer.gameObject.SetActive(false); // hide round timer
		
        StartCountdown();
	}

	// Update is called once per frame
	void Update () 
	{
		if(gameStarted)
		{
			UpdateRoundTime();
			SetCommonGoalMetIfSo();
			UpdateScoreDisplay();
		}
        else
        {
            StartCountdown();
        }
	}

    void StartCountdown()
    {
        //Subtract Time
        countdownTime -= Time.deltaTime;
        startTimer.text = Mathf.Round(countdownTime).ToString();

        if(countdownTime<0) // if countdown is complete
        {
            Destroy(startTimer.gameObject);
            roundTimer.gameObject.SetActive(true);

            gameStarted = true; // Start Game
        }
    }

	void SetCommonGoalMetIfSo()
	{
        if(GetTotalScore() > commonGoal)
		{
			PlayerPrefs.SetInt("CommonGoal", 1); 
		}
	}

	public void DogScores(float points)
	{
		dogScoreValue += points;
        PlayerPrefs.SetFloat("DogScore", dogScoreValue);
       
	}

	public void CatScores(float points)
	{
		catScoreValue += points;
        PlayerPrefs.SetFloat("CatScore", catScoreValue);
	}

	void UpdateScoreDisplay()
	{
		if (!CheckIfCommonGoalMet ()) // If Common goal isn't met
        {
            // Display Dog and Cat scores based on the Common Goal
			dogScore.fillAmount = dogScoreValue / commonGoal;
			catScore.fillAmount = catScoreValue / commonGoal;	
		}
		else // otherwise
		{
            // Display Dog and Cat scores propotionally 
            dogScore.fillAmount= dogScoreValue / GetTotalScore ();
            catScore.fillAmount = catScoreValue / GetTotalScore ();
		}

        // Display higher score bar on top
        if(dogScore.fillAmount>catScore.fillAmount)
        {
            dogScore.transform.SetAsLastSibling();
        }
        else
        {
            catScore.transform.SetAsLastSibling();
        }

        // Display Round Timer on top of HUD
        roundTimer.transform.SetAsLastSibling();
	}
        
	float GetTotalScore()
	{
		return dogScoreValue + catScoreValue;
	}

	bool CheckIfCommonGoalMet()
	{
        return GetTotalScore () >= commonGoal;
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
		SceneManager.LoadScene("GameOver");
	}
}