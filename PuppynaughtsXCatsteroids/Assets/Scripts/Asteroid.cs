using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    float mineTime = 0; 
    Testplayer myPlayer; 
	public float maxSpeed;
	public float accelerationForce = 10f;
	public float startSpeed;
	public bool playerMounted;

	float scale;

	public float points;
    public GameManager gm; 
    public GameObject pointsUI; 

	Rigidbody2D rb2D;

	// Use this for initialization
	void Start () 
    {
        gm = FindObjectOfType<GameManager>();
        myPlayer = FindObjectOfType<Testplayer>();
		scale = Random.Range (.3f, 5);
		transform.localScale = new Vector3(scale,scale,scale);

		rb2D = GetComponent<Rigidbody2D>();

		rb2D.mass = scale;

		points = 10*scale;

		rb2D.angularDrag = .05f;

		maxSpeed = 2;

		startSpeed = Random.Range (.2f, maxSpeed);

		rb2D.AddTorque (Random.Range (-3, 3));
		playerMounted = false;

		rb2D.AddForce (maxSpeed * 20 * transform.up);

    }
	// Update is called once per frame
	void Update () 
    {
        if(rb2D.velocity.magnitude > maxSpeed)
        {
            rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, maxSpeed);
        }
	}
	void FixedUpdate(){
		if(rb2D.velocity.magnitude > maxSpeed){
			rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, maxSpeed);
		}

		if (playerMounted) {
			Move ();
		}
	}
	void Move() {
		float acceleration = Input.GetAxis("Vertical");
		rb2D.AddForce(transform.up * acceleration * accelerationForce);
		Rotate();

		if (rb2D.velocity.magnitude > maxSpeed) {
			rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
		}
		//transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed);
	}
	void Rotate()	{
		transform.Rotate(0,0,Input.GetAxis("Horizontal") * Time.deltaTime * 180 * -1);
	}
    public void StartMining()
    {
        Debug.Log("Starting " + Time.time);
		StartCoroutine("MineAndDestroy");
        Debug.Log("Before Mining finishes " + Time.time);
    }

    IEnumerator MineAndDestroy()
    {
		Vector3 startScale = transform.localScale;
		float lastScale = scale;
		while (mineTime < scale*1.5f) {
			mineTime += Time.deltaTime;
			transform.localScale = Vector3.Lerp (startScale, new Vector3 (.5f, .5f, .5f), mineTime/(scale*1.5f));
			transform.position = Vector2.MoveTowards (transform.position, myPlayer.transform.position, lastScale - transform.localScale.x);
			lastScale = transform.localScale.x;
			yield return null;
		}
        myPlayer.isAttached = false;
        myPlayer.currentAsteroid = null;
        gm.SpawnPointsUI(this.gameObject.transform.position);
        Destroy(this.gameObject);

    }
}
