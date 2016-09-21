using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class EndGameManager : MonoBehaviour {


    public GameObject MissionCompletePanel;
    public GameObject MissionFailedPanel;

    public Image dogBar;
    public Image catBar;
    public Image happyCat;
    public Image happyDog;
    public Image madDog;
    public Image madCat;

    public Text winnerText; 
    public Text CatScoreText;
    public Text DogScoreText; 

    public Button menuBtn;

    float fillTime = 5.0f;
    bool fillScoreBars = false;
    bool dogBarFilled = false;
    bool catBarFilled = false;

	// Use this for initialization
	void Start () 
    {
        menuBtn.gameObject.SetActive(false);
        winnerText.gameObject.SetActive(false);
        CatScoreText.gameObject.SetActive(false);
        DogScoreText.gameObject.SetActive(false);

        CatScoreText.text = PlayerPrefs.GetFloat("CatScore").ToString("F2");
        DogScoreText.text = PlayerPrefs.GetFloat("DogScore").ToString("F2");

        // If Common Goal Met
        if(PlayerPrefs.GetInt("CommonGoal")==1)
        {
            MissionCompletePanel.SetActive(true);
            MissionFailedPanel.SetActive(false);

            GetWinner();
        }

        //If Common Goal Failed
        else
        {
            menuBtn.gameObject.SetActive(true);
            MissionFailedPanel.SetActive(true);
            MissionCompletePanel.SetActive(false);
        }
    }

    void GetWinner()
    {
        // If Cat Wins
        if(PlayerPrefs.GetFloat("CatScore") > PlayerPrefs.GetFloat("DogScore"))
        {
            winnerText.text = "Felina Wins!";
            happyCat.gameObject.SetActive(true);
            madCat.gameObject.SetActive(false);
            happyDog.gameObject.SetActive(false);
            madDog.gameObject.SetActive(true);
        }

        // If Dog Wins
        else if(PlayerPrefs.GetFloat("CatScore") < PlayerPrefs.GetFloat("DogScore"))
        {
            winnerText.text = "Barker Wins!";
            happyCat.gameObject.SetActive(false);
            madCat.gameObject.SetActive(true);
            happyDog.gameObject.SetActive(true);
            madDog.gameObject.SetActive(false);

        }
        // If Tie
        else
        {
            winnerText.text = "It's a TIE!";
            happyCat.gameObject.SetActive(true);
            madCat.gameObject.SetActive(false);
            happyDog.gameObject.SetActive(true);
            madDog.gameObject.SetActive(false);
        }

        fillScoreBars = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(fillScoreBars)
        {
            if(!dogBarFilled)
            {
                dogBar.fillAmount+=1.0f/fillTime * Time.deltaTime; 
            }
            if(!catBarFilled)
            {
                catBar.fillAmount+=1.0f/fillTime * Time.deltaTime; 
            }

            if(dogBar.fillAmount >= PlayerPrefs.GetFloat("DogScore")/(PlayerPrefs.GetFloat("DogScore") + PlayerPrefs.GetFloat("CatScore")))
            {
                dogBarFilled = true;
            }
            if(catBar.fillAmount >= PlayerPrefs.GetFloat("CatScore")/(PlayerPrefs.GetFloat("DogScore") + PlayerPrefs.GetFloat("CatScore")))
            {
                catBarFilled = true;
            }

            if(dogBarFilled && catBarFilled)
            {
                DisplayResults();
                fillScoreBars = false;
            }
        }


	}

    void DisplayResults()
    {
        winnerText.gameObject.SetActive(true);
        CatScoreText.gameObject.SetActive(true);
        DogScoreText.gameObject.SetActive(true);
        menuBtn.gameObject.SetActive(true);
    }
}
