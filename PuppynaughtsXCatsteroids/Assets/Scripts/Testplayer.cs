using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Testplayer : MonoBehaviour {

	public enum player {NoPlayer = 0, Player1 = 1, Player2 = 2};
	public Dictionary<player, string> playerHorizontalControls = new Dictionary<player, string>();
	public Dictionary<player, string> playerVerticalControls = new Dictionary<player, string>();
	public Dictionary<player, string> playerMine = new Dictionary<player, string> ();
	public player p;
	float maxBoost;
	public float maxSpeed;
	float speed;
	float driftSpeed;
	public float attachSpeed;
	public float currentSpeed; 

	public bool isAttached;
	bool isBoosting;

	public Rigidbody2D rb2D;
	public Asteroid currentAsteroid; 

	public ParticleSystem ps;

	// Use this for initialization
	void Start () 
    {
		maxSpeed = 6;
		speed = 15;

		Debug.Log (p.ToString());
		maxSpeed = 6;
		speed = 10;
		driftSpeed = 1f;
		attachSpeed = 5f;
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.angularDrag = 3;

		ps = GetComponentInChildren<ParticleSystem> ();
		playerHorizontalControls.Add (player.Player1, "P1 Horizontal");
		playerHorizontalControls.Add (player.Player2, "P2 Horizontal");
		playerVerticalControls.Add (player.Player1, "P1 Vertical");
		playerVerticalControls.Add (player.Player2, "P2 Vertical");
		playerMine.Add (player.Player1, "P1 Mine");
		playerMine.Add (player.Player2, "P2 Mine");

		isBoosting = false;
		isAttached = false;
	}

	// Update is called once per frame
	void Update () {

		Rotate();
		
        if(Input.GetButtonDown("Detach") && isAttached)
        {
            Detach();
        }
		string whichMine = playerMine[p];
		bool mine = Input.GetButtonDown (whichMine);
		if(mine && isAttached && !currentAsteroid.isMining)
        {
			Mine ();
        }



		ParticleSystem.EmissionModule em = ps.emission;

		if (Mathf.Abs(Input.GetAxis(playerVerticalControls[p])) > .01 && !isAttached) {
			em.enabled = true;
		} else {
			em.enabled = false;
		}
	}

	void FixedUpdate()
    {
        if(!isAttached)
        {
            Move();
        }
        else
        {
			maxSpeed = 4 - currentAsteroid.currentScale/2;
			Move();
        }

	}	

    void Mine()
    {
        if(currentAsteroid)
        {
            Asteroid asteroid = currentAsteroid;
            asteroid.StartMining();
        }
    }

	void Detach(){
		Revert ();
		currentAsteroid.Detach ();
	}

	public void Revert(){
		maxSpeed = 6;
		rb2D.mass = 1;
		isAttached = false;
		currentAsteroid = null;

		ParticleSystem.EmissionModule em = ps.emission;
		em.enabled = true;
	}

	void Move()
	{
		//Friction
		if (rb2D.velocity.magnitude > driftSpeed) {
			Vector3 easeVelocity = rb2D.velocity;
			easeVelocity.y *= .99f;
			easeVelocity.z = 0.0f;
			easeVelocity.x *= .99f;
			rb2D.velocity = easeVelocity; 
		}


		Vector3 keepRot = transform.eulerAngles;

		//Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		transform.eulerAngles = keepRot;

		currentSpeed = rb2D.velocity.magnitude;

		// Vertical axis for this player
		string whichVerticalAxis = playerVerticalControls[p];
		float verticalAxis = Input.GetAxis (whichVerticalAxis);

		if (currentSpeed > maxSpeed && verticalAxis !=0)
		{
			Debug.Log("Clamping");
			rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed * 1.15f);
		}
		else
		{
			rb2D.AddForce(transform.up*verticalAxis*speed);

		}
	}

	void Rotate()
	{
		// Horizontal axis for this player
		string whichHorizontalAxis = playerHorizontalControls[p];
		float horizontalAxis = Input.GetAxis (whichHorizontalAxis);
		if (!isAttached) {
			transform.Rotate (0, 0, horizontalAxis * Time.deltaTime * -180);
		} else {
			if (currentAsteroid.currentScale > 1) {
				transform.Rotate (0, 0, horizontalAxis * Time.deltaTime * -180 / currentAsteroid.currentScale);
			} else {
				transform.Rotate (0, 0, horizontalAxis * Time.deltaTime * -180);
			}
		}
	}


}
