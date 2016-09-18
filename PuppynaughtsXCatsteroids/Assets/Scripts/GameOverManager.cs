using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameOverManager : MonoBehaviour {


    public GameObject MissionCompletePanel;
    public GameObject MissionFailedPanel;

    public Image dogBar;
    public Image catBar;
    public Image happyCat;
    public Image happyDog;
    public Image madDog;
    public Image madCat;

    public Text winnerText; 

    public Button menuBtn;

    public float fillTime = 5.0f;
    bool startFilling = false;
    bool dogBarFilled = false;
    bool catBarFilled = false;
	// Use this for initialization
	void Start () 
    {
        menuBtn.gameObject.SetActive(false);
        winnerText.gameObject.SetActive(false);
        PlayerPrefs.SetFloat("CatScore", .90f);
        PlayerPrefs.SetFloat("DogScore", .30f);

        // If Common Goal Met
        if(PlayerPrefs.GetInt("CommonGoal")==0)
        {
            
            MissionCompletePanel.SetActive(true);
            MissionFailedPanel.SetActive(false);
        }

        //If Common Goal Failed
        else
        {
            menuBtn.gameObject.SetActive(true);
            MissionFailedPanel.SetActive(true);
            MissionCompletePanel.SetActive(false);
        }

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

        startFilling = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(startFilling)
        {
            if(!dogBarFilled)
            {
                dogBar.fillAmount+=1.0f/fillTime * Time.deltaTime; 
            }
            if(!catBarFilled)
            {
                catBar.fillAmount+=1.0f/fillTime * Time.deltaTime; 
            }


            if(dogBar.fillAmount >= PlayerPrefs.GetFloat("DogScore"))
            {
                dogBarFilled = true;
            }
            if(catBar.fillAmount >= PlayerPrefs.GetFloat("CatScore"))
            {
                catBarFilled = true;
            }
        }

        if(dogBarFilled && catBarFilled)
        {
            startFilling = false;
            winnerText.gameObject.SetActive(true);
            menuBtn.gameObject.SetActive(true);
        }
	}

    public void ToMenu()
    {
        SceneManager.LoadScene("GameOver");

    }
}
