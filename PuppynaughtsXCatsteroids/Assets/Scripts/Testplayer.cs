/// <summary>
/// Test Player.
/// Player controller for each player in the game.  
/// Controls all of the player's mechanics, collisions, and audio. 
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Testplayer : MonoBehaviour {

	public enum playerRef {NoPlayer = 0, Player1 = 1, Player2 = 2, XPlayer1 = 3, XPlayer2 = 4};

    public Dictionary<playerRef, string> playerHorizontalControls = new Dictionary<playerRef, string>();
    public Dictionary<playerRef, string> playerVerticalControls = new Dictionary<playerRef, string>();
    public Dictionary<playerRef, string> playerMine = new Dictionary<playerRef, string> ();
    public Dictionary<playerRef, string> playerBoost = new Dictionary<playerRef, string> ();
    public Dictionary<playerRef, string> playerDetach = new Dictionary<playerRef, string> ();
    public Dictionary<playerRef, string> playerVerticalPos = new Dictionary<playerRef, string>();
    public Dictionary<playerRef, string> playerVerticalNeg = new Dictionary<playerRef, string>();
    public Dictionary<playerRef, string> playerTaunt = new Dictionary<playerRef, string>();

    public GameManager gm; // Reference to the Game Manager
    public playerRef player; // Reference to which player this is

    float playerSpeed = 10; // Base speed for the player
    public float currentSpeed; // Player's current speed
    float maxForwardSpeed = 6.9f; // Max speed player can reach when not boosting and moving forward
    float maxBackwardSpeed = 2.1f; // Max speed player can reach when not boosting and moving backward
    float driftCheckSpeed = 1; // The speed to check if the player should be drifting
    float driftAmount = .99f; // Drift to apply to player when they reach a certain speed

    float maxBoost;

	public float attachSpeed;
   
	public bool hasAsteroid;
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

	public AudioManager am;

	void Start () 
    {
        am = FindObjectOfType<AudioManager>();
        gm = FindObjectOfType<GameManager>();

		collisionCool = false;
		detaching = false;

		attachSpeed = 5f;
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.angularDrag = 3;

		boost = GetComponent<Boost> ();

        SetUpPlayerControls();
		

		isBoosting = false;
		hasAsteroid = false;

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
	
    void SetUpPlayerControls()
    {
        playerHorizontalControls.Add (playerRef.Player1, "P1 Horizontal");
        playerHorizontalControls.Add (playerRef.Player2, "P2 Horizontal");
        playerVerticalControls.Add (playerRef.Player1, "P1 Vertical");
        playerVerticalControls.Add (playerRef.Player2, "P2 Vertical");
        playerMine.Add (playerRef.Player1, "P1 Mine");
        playerMine.Add (playerRef.Player2, "P2 Mine");
        playerBoost.Add (playerRef.Player1, "P1 Boost");
        playerBoost.Add (playerRef.Player2, "P2 Boost");
        playerDetach.Add (playerRef.Player1, "P1 Detach");
        playerDetach.Add (playerRef.Player2, "P2 Detach");
        playerHorizontalControls.Add (playerRef.XPlayer1, "P1 XBOX Hori");
        playerHorizontalControls.Add (playerRef.XPlayer2, "P2 XBOX Hori");
        playerVerticalPos.Add (playerRef.XPlayer1, "P1 R Trigger");
        playerVerticalPos.Add (playerRef.XPlayer2, "P2 R Trigger");
        playerVerticalNeg.Add (playerRef.XPlayer1, "P1 L Trigger");
        playerVerticalNeg.Add (playerRef.XPlayer2, "P2 L Trigger");
        playerMine.Add (playerRef.XPlayer1, "P1 XBOX A");
        playerMine.Add (playerRef.XPlayer2, "P2 XBOX A");
        playerBoost.Add (playerRef.XPlayer1, "P1 L Bumper");
        playerBoost.Add (playerRef.XPlayer2, "P2 L Bumper");
        playerDetach.Add (playerRef.XPlayer1, "P1 XBOX B");
        playerDetach.Add (playerRef.XPlayer2, "P2 XBOX B");
        playerTaunt.Add (playerRef.XPlayer1, "P1 XBOX Y");
        playerTaunt.Add (playerRef.XPlayer2, "P2 XBOX Y");
        playerTaunt.Add (playerRef.Player1, "P1 Taunt");
        playerTaunt.Add (playerRef.Player2, "P2 Taunt");
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
        if (Input.GetButtonDown (playerTaunt [player])) {
			PlayRandomPlayerAudio ();
		}
        if(gm.gameStarted)
        {

			Rotate();

            if(Input.GetButtonDown(playerDetach[player]) && hasAsteroid)
			{
				DetachAsteroid();
			}

            if (Input.GetButtonDown (playerBoost [player]) && !hasAsteroid) {
				boost.ShipBoost ();
				BoostCooldown ();         
			}
            string whichMine = playerMine[player];
			if(Input.GetButtonDown(whichMine) && hasAsteroid && !isMining){
                Mine();
            }

			ParticleSystem.EmissionModule em;
            if (player == playerRef.XPlayer1 || player == playerRef.XPlayer2) {
                if ((Input.GetAxis (playerVerticalPos [player]) > .01 || Input.GetAxis (playerVerticalNeg [player]) > .01) && !hasAsteroid && !isBoosting) {
					foreach (ParticleSystem sys in rocket) {
						em = sys.emission;
						em.enabled = true;

					}
					if (!rocketSound.isPlaying) {
						rocketSound.Play ();
					}
				} else {
					foreach (ParticleSystem sys in rocket) {
						em = sys.emission;
						em.enabled = false;

					}
					rocketSound.Stop();
				}
			} else {
                if (Mathf.Abs (Input.GetAxis (playerVerticalControls [player])) > .01 && !hasAsteroid && !boost.isBoosting) {
					foreach (ParticleSystem sys in rocket) {
						em = sys.emission;
						em.enabled = true;

					}
					if (!rocketSound.isPlaying) {
						rocketSound.Play ();
					}
				} else {
					foreach (ParticleSystem sys in rocket) {
						em = sys.emission;
						em.enabled = false;

					}
					rocketSound.Stop();
				}
			}

			if (boost.isBoosting) {
				foreach (ParticleSystem sys in boostP) {
					em = sys.emission;
					em.enabled = true;

				}
				if (!rocketSound.isPlaying) {
					rocketSound.Play ();
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
            if(!hasAsteroid)
            {
                Move();
            }
            else
            {
    			maxForwardSpeed = 4 - currentAsteroid.currentScale/2;
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

	void DetachAsteroid()
    {
		
		currentAsteroid.Detach ();
		currentAsteroid.rb2D.AddForce (-10*(currentAsteroid.currentScale/2)*transform.up);
		rb2D.AddForce (10 * transform.up);
		StartCoroutine ("detachCool");
		Revert ();

	}

    void BoostCooldown()
    {
        
        if(player == playerRef.Player1)
        {
            gm.catCoolDown.fillAmount = 0;
            gm.dogNeedsCoolDown = true;
        }
        else if(player == playerRef.Player2)
        {
            gm.dogCoolDown.fillAmount = 0;
            gm.catNeedsCoolDown = true;
        }

    }
	public void Revert()
    {
		maxForwardSpeed = 6.9f;
        maxBackwardSpeed = 2.1f;
		rb2D.mass = 1;
		hasAsteroid = false;
		currentAsteroid = null;
        isMining = false;
	}

	void Move()
	{
        currentSpeed = rb2D.velocity.magnitude;

        // Check To Apply Drift
        if (currentSpeed > driftCheckSpeed) 
        {
            // Apply friction
			Vector3 easeVelocity = rb2D.velocity;
			easeVelocity.y *= driftAmount;
			easeVelocity.x *= driftAmount;
			rb2D.velocity = easeVelocity; 
		}


		Vector3 keepRot = transform.eulerAngles;


		transform.eulerAngles = keepRot;

		float verticalAxis;

		// Vertical axis for this player
        if (player == playerRef.XPlayer1 || player == playerRef.XPlayer2) // If Player is using a conroller
        {
            verticalAxis = Input.GetAxis(playerVerticalPos[player]) - Input.GetAxis(playerVerticalNeg[player]); // Which direction are they going
		} 
        else // otherwise
        {
            string whichVerticalAxis = playerVerticalControls[player];
			verticalAxis = Input.GetAxis (whichVerticalAxis);
		}

        //Move Player
		if (verticalAxis < 0) // If player is moving down
        {
			if (currentSpeed > maxBackwardSpeed && !boost.isBoosting) // If player is going over max speed
            {
				rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxBackwardSpeed); // Clamp player's speed
			} 
            else // otherwise
            {
				rb2D.AddForce(transform.up*verticalAxis*playerSpeed); // Move Player down
			}
		} 
        else if(currentSpeed > maxForwardSpeed && verticalAxis !=0 && !boost.isBoosting) // otherwise, if player is moving up and going over max speed
        {
			rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxForwardSpeed); // Clamp player's speed
		}
        else // otherwise
        {
			rb2D.AddForce(transform.up*verticalAxis*playerSpeed); // Move player up

		}
            
	}

	void Rotate()
	{
		// Horizontal axis for this player
        string whichHorizontalAxis = playerHorizontalControls[player];
		float horizontalAxis = Input.GetAxis (whichHorizontalAxis);
		
        if (!hasAsteroid) // If player doesn't currently have an asteroid
        {
			transform.Rotate (0, 0, horizontalAxis * Time.deltaTime * -180); // Apply rotation
		} 
        else // otherwise
        {
			if (currentAsteroid.currentScale > 1) // If current asteroid's current scale is larger than 1
            {
				transform.Rotate (0, 0, horizontalAxis * Time.deltaTime * -180 / currentAsteroid.currentScale); // Apply weighted rotation
			} 
            else // otherwise
            {
				transform.Rotate (0, 0, horizontalAxis * Time.deltaTime * -180); // Apply normal rotation
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

	void OnCollisionEnter2D(Collision2D other)
    {
		if (other.gameObject.tag == "Player" && !collisionCool) // If collided with another player
        {
			Testplayer hitPlayer = other.gameObject.GetComponentInParent<Testplayer> (); // player hit 

			if(hitPlayer.rb2D.velocity.magnitude >= 3)
            {
                // Spawn collision particle 
				GameObject ps = Instantiate (playerHit, other.contacts[0].point, Quaternion.Euler(90,0,Mathf.Atan(Vector3.Distance(transform.position,other.transform.position)))) as GameObject;
				ParticleSystem.ShapeModule sm = ps.GetComponent<ParticleSystem> ().shape;
				sm.radius = 1f;
				ParticleSystem.EmissionModule em = ps.GetComponent<ParticleSystem> ().emission;  
			}

            if (hitPlayer.boost.isBoosting && !detaching && hasAsteroid) // If player that was hit was boosting, and this player currently has an asteroid
            {
				DetachAsteroid (); // Detach Asteroid 
			}

            StartCoroutine ("hitCool");
		}
	}

	IEnumerator hitCool()
    {
		collisionCool = true;
		yield return new WaitForSeconds (.3f);
		collisionCool = false;
	}

	IEnumerator detachCool()
    {
		detaching = true;
		yield return new WaitForSeconds (.5f);
		detaching = false;
	}
}
