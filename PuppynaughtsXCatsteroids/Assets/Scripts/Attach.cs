using UnityEngine;
using System.Collections;

public class Attach : MonoBehaviour {

    Testplayer myPlayer; 
	// Use this for initialization
	void Start () 
    {
        myPlayer = transform.parent.GetComponent<Testplayer>();

        if(myPlayer)
        {
            Debug.Log("FOund Player");
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Asteroid")
        {
            if(myPlayer.currentSpeed < myPlayer.attachSpeed)
            {
                AttachToAsteroid(other.GetComponent<Asteroid>());
 
            }
        }

    }

    void AttachToAsteroid(Asteroid asteroid)
    {
        myPlayer.isAttached = true;
		asteroid.transform.parent = myPlayer.transform;
        myPlayer.currentAsteroid = asteroid; 
		asteroid.playerMounted = true;
    }
}
