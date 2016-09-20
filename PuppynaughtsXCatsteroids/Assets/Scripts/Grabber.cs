using UnityEngine;
using System.Collections;

public class Grabber : MonoBehaviour {

    PlayerController myPlayer;

	// Use this for initialization
	void Start () 
    {
        myPlayer = transform.parent.GetComponent<PlayerController>(); // Get Player
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Asteroid") 
        {
            if(myPlayer.CanGrabAsteroid()) // If Player is currently able to grab an asteroid
            {
                Asteroid asteroid = other.GetComponentInParent<Asteroid>();

                if(!asteroid.isGrabbed) // If asteroid isn't currently being grabbed
                {
                    GrabAsteroid(asteroid); // Grab Asteroid
                }
            }
        }
    }

    void GrabAsteroid(Asteroid asteroid)
    {
        asteroid.transform.parent = myPlayer.transform; 

        myPlayer.AsteroidGrabbed(asteroid);
        asteroid.Grabbed(myPlayer); 
    }
}
