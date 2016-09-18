using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    float mineTime = 0; 
    public Testplayer myPlayer; 
	public float maxSpeed;
	public float accelerationForce = 10f;
	public float startSpeed;
	public Testplayer.player playerMounted;

	public AsteroidSpawner mom;

	float scale;
	public float currentScale;

	public float points;
    public GameManager gm; 
    public GameObject pointsUI; 

	public Rigidbody2D rb2D;

	public bool isMining;

	public Transform attatchment;


	// Use this for initialization
	void Start () 
    {
        gm = FindObjectOfType<GameManager>();
        myPlayer = FindObjectOfType<Testplayer>();
		scale = Random.Range (.3f, 5);
		currentScale = scale;
		transform.localScale = new Vector3(scale,scale,scale);
		isMining = false;

		rb2D = GetComponent<Rigidbody2D>();

		rb2D.mass = scale;

		points = 10*scale;

		rb2D.angularDrag = .05f;

		maxSpeed = 2;

		startSpeed = Random.Range (.2f, maxSpeed);

		rb2D.AddTorque (Random.Range (-3, 3));
		playerMounted = Testplayer.player.NoPlayer;

		rb2D.AddForce (maxSpeed * 20 * transform.up);

    }
	// Update is called once per frame
	void Update () 
    {
		if (playerMounted != Testplayer.player.NoPlayer) {
			if (rb2D.velocity.magnitude > maxSpeed) {
				rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
			}
				
		}
	}
	void FixedUpdate(){
		if (playerMounted != Testplayer.player.NoPlayer) {
			if (rb2D.velocity.magnitude > maxSpeed) {
				rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
			}
		}

		/*if (playerMounted != Testplayer.player.NoPlayer) {
			Move ();
		}*/
	}

	/*void Move() {
		float acceleration = Input.GetAxis("Vertical");
		rb2D.AddForce(transform.up * acceleration * accelerationForce);
		Rotate();

		if (rb2D.velocity.magnitude > maxSpeed) {
			rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
		}
		//transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed);
	}*/
		
	void Rotate()	{
		transform.Rotate(0,0,Input.GetAxis("Horizontal") * Time.deltaTime * 180 * -1);
	}
	public void StartMining(Testplayer player)
    {
        Debug.Log("Starting " + Time.time);
		StartCoroutine("MineAndDestroy");
		playerMounted = player.p;
        Debug.Log("Before Mining finishes " + Time.time);
    
	}
		

    IEnumerator MineAndDestroy()
    {
		isMining = true;
		Vector3 startScale = transform.localScale;
		float lastScale = scale;
		Transform mp = myPlayer.transform;
		while (mineTime < scale*1.5f) {
			mineTime += Time.deltaTime;
			transform.localScale = Vector3.Lerp (startScale, new Vector3 (.1f, .1f, .1f), mineTime/(scale*1.5f));
			currentScale = transform.localScale.x;
			transform.position = Vector2.MoveTowards (transform.position, mp.position-mp.up*(currentScale/2), lastScale - currentScale);
			lastScale = currentScale;
			yield return null;
		}

        myPlayer.isAttached = false;
        myPlayer.currentAsteroid = null;
        gm.SpawnPointsUI(this.gameObject.transform.position);

		myPlayer.Revert();
		mom.currentNum--;

        Destroy(this.gameObject);

    }

	public void Detach(){
		isMining = false;
		playerMounted = Testplayer.player.NoPlayer;
		myPlayer = null;
		gameObject.AddComponent<Rigidbody2D> ();
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.mass = currentScale;
	}
}
