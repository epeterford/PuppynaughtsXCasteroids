using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Testplayer : MonoBehaviour {

	public enum player {NoPlayer = 0, Player1 = 1, Player2 = 2, XPlayer1 = 3, XPlayer2 = 4};

	public Dictionary<player, string> playerHorizontalControls = new Dictionary<player, string>();
	public Dictionary<player, string> playerVerticalControls = new Dictionary<player, string>();
	public Dictionary<player, string> playerMine = new Dictionary<player, string> ();
	public Dictionary<player, string> playerBoost = new Dictionary<player, string> ();
	public Dictionary<player, string> playerDetach = new Dictionary<player, string> ();
	public Dictionary<player, string> playerVerticalPos = new Dictionary<player, string>();
	public Dictionary<player, string> playerVerticalNeg = new Dictionary<player, string>();

    public Image catBoostBar;
    public Image dogBoostBar; 
    public GameManager gm; 
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

	public ParticleSystem[] rocket;
	public ParticleSystem[] boostP;

	public GameObject playerHit;
    public GameObject PointTextPrefab;
	bool collisionCool;
	public bool detaching;

	Boost boost;

 
    public AudioSource playerAudio; 
    public AudioSource rocketSound;
    public AudioClip[] playerSounds; 
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;
	// Use this for initialization
	void Start () 
    {

        gm = FindObjectOfType<GameManager>();
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
		playerHorizontalControls.Add (player.XPlayer1, "P1 XBOX Hori");
		playerHorizontalControls.Add (player.XPlayer2, "P2 XBOX Hori");
		playerVerticalPos.Add (player.XPlayer1, "P1 R Trigger");
		playerVerticalPos.Add (player.XPlayer2, "P2 R Trigger");
		playerVerticalNeg.Add (player.XPlayer1, "P1 L Trigger");
		playerVerticalNeg.Add (player.XPlayer2, "P2 L Trigger");
		playerMine.Add (player.XPlayer1, "P1 XBOX A");
		playerMine.Add (player.XPlayer2, "P2 XBOX A");
		playerBoost.Add (player.XPlayer1, "P1 L Bumper");
		playerBoost.Add (player.XPlayer2, "P2 L Bumper");
		playerDetach.Add (player.XPlayer1, "P1 XBOX B");
		playerDetach.Add (player.XPlayer2, "P2 XBOX B");

		isBoosting = false;
		isAttached = false;

		ParticleSystem.EmissionModule em;

		foreach (ParticleSystem sys in rocket) {
			em = sys.emission;
			em.enabled = false;
		}

		foreach (ParticleSystem sys in boostP) {
			em = sys.emission;
			em.enabled = false;
		}

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
		
        if(gm.gameStarted)
        {

			Rotate();

			if(Input.GetButtonDown(playerDetach[p]) && isAttached)
			{
				Detach();
			}

			if (Input.GetButtonDown (playerBoost [p]) && !isAttached) {
				boost.ShipBoost ();
				BoostCooldown ();         
			}
			string whichMine = playerMine[p];
			if(Input.GetButtonDown(whichMine) && isAttached && !isMining){
                Mine();
            }

			ParticleSystem.EmissionModule em;
			if (p == player.XPlayer1 || p == player.XPlayer2) {
				if ((Input.GetAxis (playerVerticalPos [p]) > .01 || Input.GetAxis (playerVerticalNeg [p]) > .01) && !isAttached && !isBoosting) {
					foreach (ParticleSystem sys in rocket) {
						em = sys.emission;
						em.enabled = true;
						//rocketSound.Play();
					}
				} else {
					foreach (ParticleSystem sys in rocket) {
						em = sys.emission;
						em.enabled = false;
						//rocketSound.Stop();
					}
				}
			} else {
				if (Mathf.Abs (Input.GetAxis (playerVerticalControls [p])) > .01 && !isAttached && !boost.isBoosting) {
					foreach (ParticleSystem sys in rocket) {
						em = sys.emission;
						em.enabled = true;
						//rocketSound.Play();
					}
				} else {
					foreach (ParticleSystem sys in rocket) {
						em = sys.emission;
						em.enabled = false;
						//rocketSound.Stop();
					}
				}
			}

			if (boost.isBoosting) {
				foreach (ParticleSystem sys in boostP) {
					em = sys.emission;
					em.enabled = true;
					//rocketSound.Play();
				}
			} else {
				foreach (ParticleSystem sys in boostP) {
					em = sys.emission;
					em.enabled = false;
				}
			}
        }	
	}

	void FixedUpdate()
    {
        if(gm.gameStarted)
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

    void BoostCooldown()
    {
        
        if(p == player.Player1)
        {
            gm.dogNeedsCoolDown = true;
        }
        else if(p == player.Player2)
        {
            gm.catNeedsCoolDown = true;
        }

    }
	public void Revert(){
        Debug.Log("Reverting");
		maxSpeed = 6;
		rb2D.mass = 1;
		isAttached = false;
		currentAsteroid = null;
        isMining = false;
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

		float verticalAxis;

		// Vertical axis for this player
		if (p == player.XPlayer1 || p == player.XPlayer2) {
			verticalAxis = Input.GetAxis(playerVerticalPos[p]) - Input.GetAxis(playerVerticalNeg[p]);
		} else{
			string whichVerticalAxis = playerVerticalControls[p];
			verticalAxis = Input.GetAxis (whichVerticalAxis);
		}

		if (verticalAxis < 0) {
			if (currentSpeed > maxSpeed * .35f && !boost.isBoosting)
            {
				Vector2.ClampMagnitude (rb2D.velocity, maxSpeed * .35f);
			} 
            else 
            {
				rb2D.AddForce(transform.up*verticalAxis*speed);
			}
		} 
        else if(currentSpeed > maxSpeed && verticalAxis !=0 && !boost.isBoosting)
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
    public void PlayRandomPlayerAudio()
    {
        int randomIndex = Random.Range(0, playerSounds.Length);

        float randomPitch = Random.Range (lowPitchRange, highPitchRange);

        playerAudio.clip = playerSounds[randomIndex];
        playerAudio.pitch = randomPitch;


        playerAudio.Play();
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
