using UnityEngine;
using UnityEngine.UI;
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
    public bool isMining = false;
	public Rigidbody2D rb2D;
	public Asteroid currentAsteroid; 

	public ParticleSystem ps;

	public GameObject playerHit;
    public GameObject PointTextPrefab;
	bool collisionCool;

	Boost boost;

	// Use this for initialization
	void Start () 
    {
		maxSpeed = 6;
		speed = 15;

		collisionCool = false;

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

		isBoosting = false;
		isAttached = false;

	}
		
    public void InitPointText(string text)
    {
        GameObject temp = Instantiate(PointTextPrefab) as GameObject;
        RectTransform tempRect = temp.GetComponent<RectTransform>();

        temp.transform.SetParent(transform.FindChild("PointCanvas"));

        tempRect.transform.localPosition = PointTextPrefab.transform.localPosition;
        tempRect.transform.localScale = PointTextPrefab.transform.localScale;
        tempRect.transform.localRotation = PointTextPrefab.transform.localRotation;

        temp.GetComponent<Text>().text = text; 
        temp.GetComponent<Animator>().SetTrigger("Score");
        Destroy(temp.gameObject, 2.0f);
    }
	void Update () 
	{
		//Debug.Log (rb2D.velocity.magnitude);
		Rotate();

		if(Input.GetButtonDown("Detach") && isAttached)
		{
			Detach();
		}

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
		Revert ();
		currentAsteroid.Detach ();
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

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player" && !collisionCool) {
			Testplayer playerTemp = other.gameObject.GetComponentInParent<Testplayer> ();
			if(playerTemp.rb2D.velocity.magnitude >= 3){
				GameObject ps = Instantiate (playerHit, other.contacts[0].point, Quaternion.identity) as GameObject;
				ParticleSystem.ShapeModule sm = ps.GetComponent<ParticleSystem> ().shape;
				sm.radius = 1f;
				ParticleSystem.EmissionModule em = ps.GetComponent<ParticleSystem> ().emission;
				StartCoroutine ("hitCool");
			}
		}
	}

	IEnumerator hitCool(){
		collisionCool = true;
		yield return new WaitForSeconds (.3f);
		collisionCool = false;
	}
}
