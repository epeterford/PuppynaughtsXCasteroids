using UnityEngine;
using System.Collections;

public class Testplayer : MonoBehaviour {

	float maxSpeed;
	float speed;
	float driftSpeed;
	float maxSpinSpeed;

	Rigidbody2D rb2D;



	// Use this for initialization
	void Start () {
		maxSpeed = 3;
		speed = 10;
		driftSpeed = .5f;
		maxSpinSpeed = 1;

		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.angularDrag = 3;
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0,Input.GetAxis("Horizontal")*Time.deltaTime*180);
	}

	void FixedUpdate(){
		//Friction
		if (rb2D.velocity.magnitude > driftSpeed) {
			Vector3 easeVelocity = rb2D.velocity;
			easeVelocity.y = rb2D.velocity.y;
			easeVelocity.z = 0.0f;
			easeVelocity.x *= .85f;
		}


		Vector3 keepRot = transform.eulerAngles;

		Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		rb2D.AddForce(transform.up*Input.GetAxis("Vertical")*speed); 

		transform.eulerAngles = keepRot;

		Debug.Log (rb2D.velocity.magnitude);

		if (rb2D.velocity.magnitude > maxSpeed) {
			rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
		}


	}
}
