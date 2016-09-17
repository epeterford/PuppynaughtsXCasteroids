using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Image dogScore; 
    public Image catScore; 
    public Text roundTimer; 
    public float timeLeft; 
	// Use this for initialization
	void Start () 
    {
        dogScore.fillAmount = 1f;
        catScore.fillAmount = 1f;
        timeLeft = 60;
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateCurrentScore();

        UpdateRoundTime();

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
