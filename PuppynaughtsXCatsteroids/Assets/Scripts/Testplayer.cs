using UnityEngine;
using System.Collections;

public class Testplayer : MonoBehaviour {

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
    {
		maxSpeed = 6;
		speed = 15;
		driftSpeed = 1f;
        attachSpeed = 5f;
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.angularDrag = 2;
	
        isBoosting = false;
        isAttached = false;
	}
	
	// Update is called once per frame
	void Update () 
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
}
