using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Image dogScore; 
	public Image catScore; 
	public float dogScoreValue;
	public float catScoreValue;
	public Text roundTimer; 
	public float timeLeft; 
	public GameObject scoreUI;
	public Image dogCoolDown;
	public Image catCoolDown; 
	public bool catNeedsCoolDown = false;
	public bool dogNeedsCoolDown = false;
	public float commonGoal;
	public float uiScoreScaling;
	static bool commonGoalMet = false;
	// Use this for initialization
	void Start () 
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetInt("CommonGoal", 0);
		dogScore.fillAmount = 0f;
		catScore.fillAmount = 0f;
		dogScoreValue = 98f;
		catScoreValue = 98f;
		timeLeft = 60;
		commonGoal = 200;
	}

	// Update is called once per frame
	void Update () 
	{
		UpdateCurrentScore();

		UpdateRoundTime();

		SetCommonGoalMetIfSo();

		UpdateScoreDisplay ();

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

	void SetCommonGoalMetIfSo()
	{
		if(TotalScore() > 100)
		{
			PlayerPrefs.SetInt("CommonGoal", 1); 
			commonGoalMet = true; 
		}
	}

	public void DogScores(float points)
	{
		dogScoreValue += points;
		UpdateScoreDisplay ();
		Debug.Log("dog given: " + points);
	}

	public void CatScores(float points)
	{
		catScoreValue += points;
		UpdateScoreDisplay ();
		Debug.Log("dog given: " + points);
	}
	public void UpdateScoreDisplay()
	{
		if (!CheckIfCommonGoalMet ())
		{
			dogScore.fillAmount = dogScoreValue / commonGoal;
			catScore.fillAmount = catScoreValue / commonGoal;	
		}
		else
		{
			float dogProportionalScore;
			float catProportionalScore;

			dogProportionalScore = dogScoreValue / TotalScore ();
			catProportionalScore = catScoreValue / TotalScore ();
			dogScore.fillAmount = dogProportionalScore;
			catScore.fillAmount = catProportionalScore;
		}
	}
	public float TotalScore()
	{
		return dogScoreValue + catScoreValue;
	}
	public bool CheckIfCommonGoalMet()
	{
		return TotalScore () >= commonGoal;
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
		//SceneManager.LoadScene("GameOver");
	}

}