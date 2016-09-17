using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed = 6f;            // The speed that the player will move at.

	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Animator anim;                      // Reference to the animator component.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	float camRayLength = 100f; 
	private Vector3 startPos; 
    public float accelerationForce = 10f;
    public float rotationForce = 3f;
	private float playerSpeed = .2f;
    private float maxSpeed = 3f;
	void Start()
	{
		startPos = transform.position;
	}
	void Awake ()
	{
		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");

		// Set up references.
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}
	void Update()
	{


	}
	void FixedUpdate()
	{
		// Store the input axes.
		float v = Input.GetAxisRaw ("Vertical");


		// Move the player around the scene.
		Move ();

		// Animate the player.
		//Animating (h, v);
	}

	void Rotate()
	{
		transform.Rotate(0,0,Input.GetAxis("Horizontal") * Time.deltaTime * 180);
	}
	void Move ()
	{
        float acceleration = Input.GetAxis("Vertical");
        playerRigidbody.AddForce(transform.up * acceleration * accelerationForce);
        Rotate();

        if (playerRigidbody.velocity.magnitude > maxSpeed) {
            playerRigidbody.velocity = Vector2.ClampMagnitude (playerRigidbody.velocity, maxSpeed);
        }
		//transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed);
	}


	void Animating (float h, float v)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool bMoving = h != 0f || v != 0f;

		// Tell the animator whether or not the player is walking.
		anim.SetBool ("IsMoving", bMoving);
	}
		

}
