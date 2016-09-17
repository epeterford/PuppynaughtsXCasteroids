using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float maxSpeed;
	public float startSpeed;

	float scale;

	public float points;

	Rigidbody2D rb2D;

	// Use this for initialization
	void Start () {
		scale = Random.Range (.1f, 2);
		transform.localScale = new Vector3(scale,scale,scale);

		rb2D = GetComponent<Rigidbody2D>();

		rb2D.mass = scale;

		points = 10*scale;

		rb2D.angularDrag = 2;

		maxSpeed = 2;

		startSpeed = Random.Range (.2f, maxSpeed);

		rb2D.AddForce (maxSpeed * 20 * transform.up);


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		if(rb2D.velocity.magnitude > maxSpeed){
			rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, maxSpeed);
		}
	}
		
}
