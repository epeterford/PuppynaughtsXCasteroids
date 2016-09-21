using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public AsteroidSpawner mom;
    PlayerController myPlayer; 
    GameManager gm; 
    public ScreenWrap sw;
    MeshRenderer mr;
    public AudioManager am;

    float mineTime = 0; // Time it takes to mine asteroid

	float maxSpeed; // Max Speed of asteroid
    float driftSpeed;

	float scale; // Asteroid base scale
	public float currentScale; // Asteroid current scale

	public float points; // Asteroid's points
    GameObject pointsUI; 

	public Rigidbody2D rb2D;

	public Transform attatchment;

	public GameObject boomP; // Destruction Particle
	public GameObject noseHitP; // Collision with player particle

	float killTime = 0; // Time to destroy asteroid 

    public bool isGrabbed;
	bool collisionCool;

	// Use this for initialization
	void Start () 
    {
		sw = GetComponent<ScreenWrap> ();
        gm = FindObjectOfType<GameManager>();
        rb2D = GetComponent<Rigidbody2D>();
        myPlayer = FindObjectOfType<PlayerController>();
        mr = GetComponentInChildren<MeshRenderer> ();
        am = GameObject.FindGameObjectWithTag ("Audio").GetComponent<AudioManager>();

		scale = Random.Range (.3f, 5);
		currentScale = scale;
		transform.localScale = new Vector3(scale,scale,scale);

        maxSpeed = 2;
		driftSpeed = Random.Range (.2f, 2f);

        // Add initial force and torque to Asteroid 
        rb2D.AddTorque (Random.Range (-10, 10)*scale);
        rb2D.AddForce (maxSpeed * 20 * transform.up);

		collisionCool = false;
        isGrabbed = false;

		rb2D.mass = scale;
        rb2D.angularDrag = .05f;

		points = 10*scale;
    }
	// Update is called once per frame
	void Update () 
    {
		if (!mr.isVisible) // If Asteroid isn't visible
        {
			killTime += Time.deltaTime; // Start kill time
			if (killTime > 5) // If kill time is more than 5 seconds
            {
                // Desroy asteroid
				mom.currentNum--;
				Destroy (gameObject);
			}
		} 
        else // otherwise
        {
			killTime = 0; // reset Kill Time
		}
	}

	void FixedUpdate()
    {
		if (!myPlayer) // If there is no current player grabbing this Asteroid
        {
			if (rb2D.velocity.magnitude > maxSpeed) // If Asteroid is going faster than Max Speed
            {
				rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed); // Clamp Asteroid's velocity to Max Speed
			}

			if (rb2D.velocity.magnitude > driftSpeed) // If Asteroid is going faster than Drift Speed
			{
                // Apply friction to Asteroid 
				Vector3 easeVelocity = rb2D.velocity;
				easeVelocity.y *= .999f;
				easeVelocity.z = 0.0f;
				easeVelocity.x *= .999f;
				rb2D.velocity = easeVelocity; 
			}
		}
	}
		
	void Rotate()	
    {
		transform.Rotate(0,0,Input.GetAxis("Horizontal") * Time.deltaTime * 180 * -1);
	}

	public void StartMining()
    {
		StartCoroutine("MineAndDestroy");
	}

	void OnCollisionEnter2D(Collision2D other)
    {
		if (other.gameObject.tag == "Player" && !collisionCool) // If Asteroid collides with a Player
        {
			PlayerController player = other.gameObject.GetComponentInParent<PlayerController> (); // Get Player

			if(player.rb2D.velocity.magnitude >= 3) // If Player is going faster than 3
            {
                // Spawn Nose Hit Particle
				GameObject ps = Instantiate (noseHitP, other.contacts[0].point, Quaternion.identity) as GameObject;
				ParticleSystem.ShapeModule sm = ps.GetComponent<ParticleSystem> ().shape;
				sm.radius = .2f;
				ParticleSystem.EmissionModule em = ps.GetComponent<ParticleSystem> ().emission;
				
                StartCoroutine ("HitCool");
			}
		}
	}
		
    IEnumerator MineAndDestroy()
    {
		Vector3 startScale = transform.localScale; // Get Asteroid's starting scale at this moment
		float lastScale = scale;
		Transform player = myPlayer.transform;

        // Start Mining
		while (mineTime < scale*1.5f) 
        {
            // Mine(Scale down) Asteroid over time
			mineTime += Time.deltaTime;
			transform.localScale = Vector3.Lerp (startScale, new Vector3 (.1f, .1f, .1f), mineTime/(scale*1.5f));
			currentScale = transform.localScale.x;
			transform.position = Vector2.MoveTowards (transform.position, player.position-player.up*(currentScale/2), lastScale - currentScale);
			lastScale = currentScale;

			myPlayer.rb2D.mass = 1 + scale; // Update Player's mass
			yield return null;
		}

		myPlayer.Revert(); // Reset Player
		mom.currentNum--; // Update Asteroid Tracker
        GivePoints(points); // Give Points
        SpawnAsteroidDestroyedParticle();

        Destroy(this.gameObject); // Destroy Asteroid
    }

	IEnumerator HitCool()
    {
		collisionCool = true;
		yield return new WaitForSeconds (.3f);
		collisionCool = false;
	}
    public void Grabbed(PlayerController player)
    {
        isGrabbed = true;
        sw.enabled = false;
        myPlayer = player;
        Destroy(GetComponent<Rigidbody2D>()); 
    }

    void SpawnAsteroidDestroyedParticle()
    {
        GameObject ps = Instantiate (boomP, transform.position, Quaternion.identity) as GameObject;
        ParticleSystem.ShapeModule sm = ps.GetComponent<ParticleSystem> ().shape;
        sm.radius = .3f * scale;
        ParticleSystem.EmissionModule em = ps.GetComponent<ParticleSystem> ().emission;
        em.enabled = true;
    }
	public void Detach()
    {
		myPlayer = null;
		sw.enabled = true;
		isGrabbed = false;
		transform.parent = null;
		gameObject.AddComponent<Rigidbody2D> ();
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.mass = currentScale;

		StopCoroutine ("MineAndDestroy");
	}

    void GivePoints(float points)
    {
        if(myPlayer.player == PlayerController.playerRef.Player1 || myPlayer.player == PlayerController.playerRef.XPlayer1 )
        {
            // Dog scores
            gm.DogScores(points);
        }
        else if(myPlayer.player == PlayerController.playerRef.Player2 || myPlayer.player == PlayerController.playerRef.XPlayer2 )
        {
            // Cat scores
            gm.CatScores(points);
        }

        myPlayer.InitPointText(Mathf.Round(points).ToString());
    }
}
