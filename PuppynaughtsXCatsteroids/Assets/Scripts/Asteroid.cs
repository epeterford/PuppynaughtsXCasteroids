using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    float mineTime = 0; 
    public Testplayer myPlayer; 
	public float maxSpeed;
	public float accelerationForce = 10f;
	public float startSpeed;

	public AsteroidSpawner mom;

	float scale;
	public float currentScale;

	public float points;
    public GameManager gm; 
    public GameObject pointsUI; 

	public Rigidbody2D rb2D;

	public bool isMining;

	public Transform attatchment;

	public GameObject boom;

	public GameObject badLand;
	public GameObject noseHit;

	float killTime;
	MeshRenderer mr;

	bool collisionCool;

	// Use this for initialization
	void Start () 
    {
        gm = FindObjectOfType<GameManager>();
        myPlayer = FindObjectOfType<Testplayer>();
		scale = Random.Range (.3f, 5);
		currentScale = scale;
		transform.localScale = new Vector3(scale,scale,scale);
		isMining = false;

		mr = GetComponent<MeshRenderer> ();

		killTime = 0;

		collisionCool = false;

		rb2D = GetComponent<Rigidbody2D>();

		rb2D.mass = scale;

		points = 10*scale;

		rb2D.angularDrag = .05f;

		maxSpeed = 2;

		startSpeed = Random.Range (.2f, maxSpeed);

		rb2D.AddTorque (Random.Range (-3, 3));
		rb2D.AddForce (maxSpeed * 20 * transform.up);

    }
	// Update is called once per frame
	void Update () 
    {
		if (!mr.isVisible) {
			killTime += Time.deltaTime;
			if (killTime > 5) {
				mom.currentNum--;
				Destroy (gameObject);
			}
		} else {
			killTime = 0;
		}
	}

	void FixedUpdate(){

		if (!myPlayer)
        {
			if (rb2D.velocity.magnitude > maxSpeed) 
            {
				rb2D.velocity = Vector2.ClampMagnitude (rb2D.velocity, maxSpeed);
			}
				
		}
	}
		
	void Rotate()	{
		transform.Rotate(0,0,Input.GetAxis("Horizontal") * Time.deltaTime * 180 * -1);
	}
	public void StartMining()
    {
		StartCoroutine("MineAndDestroy");

	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player" && !collisionCool) {
			Testplayer playerTemp = other.gameObject.GetComponentInParent<Testplayer> ();
			if(playerTemp.rb2D.velocity.magnitude >= 3){
				GameObject ps = Instantiate (noseHit, other.contacts[0].point, Quaternion.identity) as GameObject;
				ParticleSystem.ShapeModule sm = ps.GetComponent<ParticleSystem> ().shape;
				sm.radius = .2f;
				ParticleSystem.EmissionModule em = ps.GetComponent<ParticleSystem> ().emission;
				StartCoroutine ("hitCool");
			}
		}
	}
		

    IEnumerator MineAndDestroy()
    {
		isMining = true;
		Vector3 startScale = transform.localScale;
		float lastScale = scale;
		Transform mp = myPlayer.transform;
		while (mineTime < scale*1.5f) 
        {
			mineTime += Time.deltaTime;
			transform.localScale = Vector3.Lerp (startScale, new Vector3 (.1f, .1f, .1f), mineTime/(scale*1.5f));
			currentScale = transform.localScale.x;
			transform.position = Vector2.MoveTowards (transform.position, mp.position-mp.up*(currentScale/2), lastScale - currentScale);
			lastScale = currentScale;
			myPlayer.rb2D.mass = 1 + scale;
			yield return null;
		}
            

		myPlayer.Revert();
		mom.currentNum--;
        GivePoints(points);
		GameObject ps = Instantiate (boom, transform.position, Quaternion.identity) as GameObject;
		ParticleSystem.ShapeModule sm = ps.GetComponent<ParticleSystem> ().shape;
		sm.radius = .3f * scale;
		ParticleSystem.EmissionModule em = ps.GetComponent<ParticleSystem> ().emission;
		em.enabled = true;

        Destroy(this.gameObject);

    }

	IEnumerator hitCool(){
		collisionCool = true;
		yield return new WaitForSeconds (.3f);
		collisionCool = false;
	}

	public void Detach()
    {
		isMining = false;
		myPlayer = null;
		gameObject.AddComponent<Rigidbody2D> ();
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.mass = currentScale;
	}

    void GivePoints(float points)
    {
        if(myPlayer.p == Testplayer.player.Player1)
        {
            // dog scores
            Debug.Log("DogScores!");
            gm.DogScores(points/200);
        }
        else if(myPlayer.p == Testplayer.player.Player2)
        {
            // cat scores
            Debug.Log("CatScores!");
            gm.CatScores(points/200);
           
        }
        Debug.Log("Points Given: " + points);
        Debug.Log("Actual Points Given: " + points/200);

    }
}
