using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour {
	
	float maxBoost;
	float maxSpeed;
	float speed;
	float driftSpeed;
	public float attachSpeed;
	public float currentSpeed;
	float timeSinceBoost;
	
	public bool isAttached;
	public bool isBoosting;
	
	Rigidbody2D rb2D;
	
	
	
	// Use this for initialization
	void Start () 
	{
		maxSpeed = 3;
		maxBoost = 25;
		speed = 10;
		driftSpeed = .2f;
		attachSpeed = 1.5f;
		timeSinceBoost = 0;
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.angularDrag = 3;
		
		isBoosting = false;
		isAttached = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Rotate();
	}
	
	void FixedUpdate()
	{
		ShipBoost();
		Move();
	}
	
	void Move()
	{
		currentSpeed = rb2D.velocity.magnitude;

		//Friction
		if (currentSpeed > driftSpeed) {
			Vector3 easeVelocity = rb2D.velocity;
			easeVelocity.y = rb2D.velocity.y;
			easeVelocity.z = 0.0f;
			easeVelocity.x *= .7f;
		}
		
		
		Vector3 keepRot = transform.eulerAngles;
		
		Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		rb2D.AddForce(transform.up*Input.GetAxis("Vertical")*speed); 
		
		transform.eulerAngles = keepRot;
		
		Debug.Log (currentSpeed);
		
		if (isBoosting == true) {
			if (timeSinceBoost > 1) {
				// Boost will cut off when timeSinceBoost == 1.5, so this formula ramps down velocity to just under 3 (maxSpeed).
				rb2D.velocity = new Vector2((maxBoost/Mathf.Pow(timeSinceBoost,6))*transform.up.x,(maxBoost/Mathf.Pow(timeSinceBoost,6))*transform.up.y);
			} else {
				// velocity is immediately set to maxBoost (25).
				rb2D.velocity = new Vector2(maxBoost*transform.up.x,maxBoost*transform.up.y);
			}
		} else if (currentSpeed > maxSpeed) {
			rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
		}
	}
	
	void Rotate()
	{
		transform.Rotate(0,0,Input.GetAxis("Horizontal")*Time.deltaTime*-180);
	}
	
	void ShipBoost()
	{
		timeSinceBoost += Time.deltaTime;
		if (timeSinceBoost > 1.5) {
			isBoosting = false;
		}
		if (Input.GetKeyDown(KeyCode.Space) && timeSinceBoost > 3) {
			timeSinceBoost = 0;
			isBoosting = true;
		}
	}
}
