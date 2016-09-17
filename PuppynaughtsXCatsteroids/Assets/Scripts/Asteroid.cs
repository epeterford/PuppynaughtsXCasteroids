using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    float mineTime = 5; 
    Testplayer myPlayer; 
   
	// Use this for initialization
	void Start () 
    {
        myPlayer = FindObjectOfType<Testplayer>();
       // StartMining(); 
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void StartMining()
    {
        Debug.Log("Starting " + Time.time);
        StartCoroutine(MineAndDestroy(mineTime));
        Debug.Log("Before Mining finishes " + Time.time);
    }

    IEnumerator MineAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("WaitAndPrint " + Time.time);
        myPlayer.isAttached = false;
        myPlayer.transform.parent = null;
        Destroy(this.gameObject);

    }
}
