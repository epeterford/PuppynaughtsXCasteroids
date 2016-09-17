using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    float mineTime = 5; 
    Testplayer myPlayer; 
	public float maxSpeed;
	public float accelerationForce = 10f;
	public float startSpeed;
	public bool playerMounted;

	float scale;

	public float points;

	Rigidbody2D rb2D;

	// Use this for initialization
	void Start () 
    {
        myPlayer = FindObjectOfType<Testplayer>();
		scale = Random.Range (.1f, 2);
		transform.localScale = new Vector3(scale,scale,scale);

		rb2D = GetComponent<Rigidbody2D>();

		rb2D.mass = scale;

		points = 10*scale;

		rb2D.angularDrag = 2;

		maxSpeed = 2;

		startSpeed = Random.Range (.2f, maxSpeed);

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
        StartCoroutine(MineAndDestroy(mineTime));
        Debug.Log("Before Mining finishes " + Time.time);
    }

    IEnumerator MineAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("WaitAndPrint " + Time.time);
        myPlayer.isAttached = false;
        myPlayer.transform.parent = null;
        myPlayer.currentAsteroid = null;
        Destroy(this.gameObject);

    }
}
