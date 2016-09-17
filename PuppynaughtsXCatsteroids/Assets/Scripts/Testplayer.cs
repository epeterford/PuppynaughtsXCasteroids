using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Testplayer : MonoBehaviour {

	public enum player {Player1 = 0, Player2 = 1};
	public Dictionary<string, string> playerHorizontalControls = new Dictionary<string, string>();
	public Dictionary<string, string> playerVerticalControls = new Dictionary<string, string>();
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


	// Use this for initialization
	void Start () 
<<<<<<< HEAD
    {
		maxSpeed = 6;
		speed = 15;
=======
	{
		Debug.Log (p.ToString());
		maxSpeed = 6;
		speed = 10;
>>>>>>> d3d261c1dc948bd00980c36c8d4c76371eb96a30
		driftSpeed = 1f;
		attachSpeed = 5f;
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.angularDrag = 3;

		playerHorizontalControls.Add ("Player1", "P1 Horizontal");
		playerHorizontalControls.Add ("Player2", "P2 Horizontal");
		playerVerticalControls.Add ("Player1", "P1 Vertical");
		playerVerticalControls.Add ("Player2", "P2 Vertical");

		isBoosting = false;
		isAttached = false;
	}

	// Update is called once per frame
	void Update () 
<<<<<<< HEAD
    {
		if (!isAttached) {
			Rotate ();
		} else {
			//RotatewithCargo ();
			Rotate();
		}
        if(Input.GetButtonDown("Detach") && isAttached)
        {
            Detatch();
        }
        if(Input.GetButtonDown("Mine") && isAttached)
        {
			if (!currentAsteroid.isMining) {
				Mine ();
			}
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

        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.eulerAngles = keepRot;

        currentSpeed = rb2D.velocity.magnitude;
        if (currentSpeed > maxSpeed && Input.GetAxis("Vertical") !=0)
        {
            Debug.Log("Clamping");
            rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
        }
        else
        {
            rb2D.AddForce(transform.up*Input.GetAxis("Vertical")*speed);

        }
    }

	void RotatewithCargo(){
		transform.RotateAround (transform.position,new Vector3(0,0,currentAsteroid.transform.position.z),Input.GetAxis("Horizontal")*Time.deltaTime*-180);
	}

    void Rotate()
    {
        transform.Rotate(0,0,Input.GetAxis("Horizontal")*Time.deltaTime*-180);
    }
		

    void Mine()
    {
        if(currentAsteroid)
        {
            Asteroid asteroid = currentAsteroid;
            asteroid.StartMining();
        }
    }

	void Detatch(){
		Revert ();
		currentAsteroid.Detatch ();
	}

	public void Revert(){
		maxSpeed = 6;
		rb2D.mass = 1;
		isAttached = false;
		currentAsteroid = null;

	}
=======
	{
		if(!isAttached)
		{
			Rotate();
		}
		if(Input.GetButtonDown("Detach") && isAttached)
		{
			Detach();
		}
		if(Input.GetButtonDown("Mine") && isAttached)
		{
			Mine();
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
			rb2D.velocity = currentAsteroid.GetComponent<Rigidbody2D>().velocity; 
		}

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
		string whichVerticalAxis = playerVerticalControls[p.ToString()];
		float verticalAxis = Input.GetAxis (whichVerticalAxis);

		if (currentSpeed > maxSpeed && verticalAxis !=0)
		{
			Debug.Log("Clamping");
			rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
		}
		else
		{
			rb2D.AddForce(transform.up*verticalAxis*speed);

		}
	}

	void Rotate()
	{
		// Horizontal axis for this player
		string whichHorizontalAxis = playerHorizontalControls[p.ToString()];
		float horizontalAxis = Input.GetAxis (whichHorizontalAxis);
		transform.Rotate(0,0,horizontalAxis*Time.deltaTime*-180);
	}

	void Detach()
	{
		isAttached = false;
		transform.parent = null;
		currentAsteroid = null;

	}

	void Mine()
	{
		if(currentAsteroid)
		{
			Asteroid asteroid = currentAsteroid;
			asteroid.StartMining();
		}
	}
>>>>>>> d3d261c1dc948bd00980c36c8d4c76371eb96a30
}
