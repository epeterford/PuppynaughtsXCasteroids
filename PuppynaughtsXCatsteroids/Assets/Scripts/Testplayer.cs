using UnityEngine;
using System.Collections;

public class Testplayer : MonoBehaviour {

	float maxBoost;
	float maxSpeed;
	float speed;
	float driftSpeed;
    public float attachSpeed;
    public float currentSpeed; 

    public bool isAttached;
	bool isBoosting;

	public Rigidbody2D rb2D;



	// Use this for initialization
	void Start () 
    {
		maxSpeed = 3;
		speed = 10;
		driftSpeed = .2f;
        attachSpeed = 5f;
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.angularDrag = 3;
	
        isBoosting = false;
        isAttached = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(!isAttached)
        {
            Rotate();
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
            rb2D.velocity = new Vector3(0,0,0);
        }

	}

    void Move()
    {
        //Friction
        if (rb2D.velocity.magnitude > driftSpeed) {
            Vector3 easeVelocity = rb2D.velocity;
            easeVelocity.y = rb2D.velocity.y;
            easeVelocity.z = 0.0f;
            easeVelocity.x *= .7f;
        }


        Vector3 keepRot = transform.eulerAngles;

        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb2D.AddForce(transform.up*Input.GetAxis("Vertical")*speed); 

        transform.eulerAngles = keepRot;

        currentSpeed = rb2D.velocity.magnitude;
       // Debug.Log (rb2D.velocity.magnitude);

        if (currentSpeed > maxSpeed) 
        {
            rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
        }
    }

    void Rotate()
    {
        transform.Rotate(0,0,Input.GetAxis("Horizontal")*Time.deltaTime*-180);
    }
}
