using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float maxSpeed;

	float scale;

	public float points;

	Rigidbody2D rb2D;

	// Use this for initialization
	void Start () {
		scale = Random.Range (.1f, 2);
		transform.localScale = new Vector3(scale,scale,scale);

		points = 10*scale;

		rb2D = GetComponent<Rigidbody2D>();

		rb2D.angularDrag = 2;

		maxSpeed = 2;

		rb2D.velocity = new Vector2(Random.Range(0,maxSpeed),Random.Range(0,maxSpeed));

		if(rb2D.velocity.magnitude > maxSpeed){
			rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, maxSpeed);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
