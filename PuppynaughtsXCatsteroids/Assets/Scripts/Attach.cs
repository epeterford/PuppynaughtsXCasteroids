using UnityEngine;
using System.Collections;

public class Attach : MonoBehaviour {

    Testplayer myPlayer;

	// Use this for initialization
	void Start () 
    {
        myPlayer = transform.parent.GetComponent<Testplayer>();

	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		if(other.tag=="Asteroid" && !myPlayer.hasAsteroid)
        {
			if(myPlayer.currentSpeed < myPlayer.attachSpeed && !myPlayer.hasAsteroid && !myPlayer.detaching)
            {
                AttachToAsteroid(other.GetComponentInParent<Asteroid>());
 
            }
        }

    }

    void AttachToAsteroid(Asteroid asteroid)
    {
		if (!asteroid.isMounted) {
			Debug.Log ("Attaching");
			ParticleSystem.EmissionModule em;
			asteroid.isMounted = true;
			foreach (ParticleSystem sys in myPlayer.rocket) {
				em = sys.emission;
				em.enabled = false;
			}

			asteroid.sw.enabled = false;
			myPlayer.hasAsteroid = true;
			myPlayer.rb2D.mass += asteroid.rb2D.mass;
			myPlayer.rb2D.velocity += asteroid.rb2D.velocity;
			asteroid.transform.parent = myPlayer.transform;
			asteroid.rb2D = null;
			asteroid.attatchment = transform;
			Destroy (asteroid.GetComponent<Rigidbody2D> ());
			myPlayer.currentAsteroid = asteroid; 
			asteroid.myPlayer = myPlayer;
		}
    }


}
