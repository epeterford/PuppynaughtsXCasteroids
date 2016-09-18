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
			if(myPlayer.currentSpeed < myPlayer.attachSpeed && !myPlayer.isAttached)
            {
                AttachToAsteroid(other.GetComponent<Asteroid>());
 
            }
        }

    }

    void AttachToAsteroid(Asteroid asteroid)
    {
		ParticleSystem.EmissionModule em = myPlayer.ps.emission;
		em.enabled = false;

        myPlayer.isAttached = true;
		myPlayer.rb2D.mass += asteroid.rb2D.mass;
		myPlayer.rb2D.velocity += asteroid.rb2D.velocity;
		asteroid.transform.parent = myPlayer.transform;
		asteroid.rb2D = null;
		asteroid.attatchment = transform;
		Destroy(asteroid.GetComponent<Rigidbody2D>());
        myPlayer.currentAsteroid = asteroid; 
		asteroid.myPlayer = myPlayer;
    }
}
