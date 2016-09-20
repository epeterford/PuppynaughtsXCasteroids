using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour {

	public void ToMainLevel()
	{
		SceneManager.LoadScene("MainLevelScene");	
	}

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");   
    }
        
}