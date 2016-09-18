using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Testplayer : MonoBehaviour {

	public enum player {NoPlayer = 0, Player1 = 1, Player2 = 2};

	public Dictionary<player, string> playerHorizontalControls = new Dictionary<player, string>();
	public Dictionary<player, string> playerVerticalControls = new Dictionary<player, string>();
	public Dictionary<player, string> playerMine = new Dictionary<player, string> ();
	public Dictionary<player, string> playerBoost = new Dictionary<player, string> ();
	public Dictionary<player, string> playerDetach = new Dictionary<player, string> ();

	public player p;
	float maxBoost;
	public float maxSpeed;
	float speed;
	float driftSpeed;
	public float attachSpeed;
	public float currentSpeed; 

	public bool isAttached;
	bool isBoosting;
    public bool isMining = false;
	public Rigidbody2D rb2D;
	public Asteroid currentAsteroid; 

	public ParticleSystem ps;

	public GameObject playerHit;

	bool collisionCool;
	public bool detaching;

	Boost boost;

	// Use this for initialization
	void Start () 
    {
		maxSpeed = 6;
		speed = 15;

		collisionCool = false;
		detaching = false;

		Debug.Log (p.ToString());
		maxSpeed = 6;
		speed = 10;
		driftSpeed = 1f;
		attachSpeed = 5f;
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.angularDrag = 3;

		ps = GetComponentInChildren<ParticleSystem> ();

		boost = GetComponent<Boost> ();

		playerHorizontalControls.Add (player.Player1, "P1 Horizontal");
		playerHorizontalControls.Add (player.Player2, "P2 Horizontal");
		playerVerticalControls.Add (player.Player1, "P1 Vertical");
		playerVerticalControls.Add (player.Player2, "P2 Vertical");
		playerMine.Add (player.Player1, "P1 Mine");
		playerMine.Add (player.Player2, "P2 Mine");
		playerBoost.Add (player.Player1, "P1 Boost");
		playerBoost.Add (player.Player2, "P2 Boost");
		playerDetach.Add (player.Player1, "P1 Detach");
		playerDetach.Add (player.Player2, "P2 Detach");

		isBoosting = false;
		isAttached = false;
	}
		

	void Update () 
	{
		Rotate();

		if(Input.GetButtonDown(playerDetach[p]) && isAttached)
		{
			Detach();
		}

		/*if(Input.GetButtonDown(playerBoost[p]) && !isAttached)
		{
			boost.ShipBoost();
		}*/

		string whichMine = playerMine[p];
        if(Input.GetButtonDown(whichMine) && isAttached && !isMining)
        {
            Mine();
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
            isMining = true;
            Asteroid asteroid = currentAsteroid;
            asteroid.StartMining();
        }
    }

	void Detach()
    {
		
		currentAsteroid.Detach ();
		currentAsteroid.rb2D.AddForce (-10*(currentAsteroid.currentScale/2)*transform.up);
		rb2D.AddForce (10 * transform.up);
		StartCoroutine ("detachCool");
		Revert ();

	}

	public void Revert(){
        Debug.Log("Reverting");
		maxSpeed = 6;
		rb2D.mass = 1;
		isAttached = false;
		currentAsteroid = null;
        isMining = false;
		ParticleSystem.EmissionModule em = ps.emission;
		em.enabled = true;
	}

	void Move()
	{
		//Friction
		if (rb2D.velocity.magnitude > driftSpeed)
        {
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

		if (currentSpeed > maxSpeed && verticalAxis !=0 && !boost.isBoosting)
		{
			rb2D.velocity = (verticalAxis > 0) ? Vector2.ClampMagnitude (rb2D.velocity, maxSpeed * 1.15f) : Vector2.ClampMagnitude (rb2D.velocity, maxSpeed * .5f);
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

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player" && !collisionCool) {
			Testplayer playerTemp = other.gameObject.GetComponentInParent<Testplayer> ();
			if(playerTemp.rb2D.velocity.magnitude >= 3){
				GameObject ps = Instantiate (playerHit, other.contacts[0].point, Quaternion.Euler(90,0,Mathf.Atan(Vector3.Distance(transform.position,other.transform.position)))) as GameObject;
				ParticleSystem.ShapeModule sm = ps.GetComponent<ParticleSystem> ().shape;
				sm.radius = 1f;
				ParticleSystem.EmissionModule em = ps.GetComponent<ParticleSystem> ().emission;
				StartCoroutine ("hitCool");
			}
			if (playerTemp.boost.isBoosting && !detaching && playerTemp.isAttached) {
				Detach ();
				StartCoroutine ("detachCool");
			}
		}
	}

	IEnumerator hitCool(){
		collisionCool = true;
		yield return new WaitForSeconds (.3f);
		collisionCool = false;
	}

	IEnumerator detachCool(){
		Debug.Log ("Running");
		detaching = true;
		yield return new WaitForSeconds (.5f);
		detaching = false;
	}
}
