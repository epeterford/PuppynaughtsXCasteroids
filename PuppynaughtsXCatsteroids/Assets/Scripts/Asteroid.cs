using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    float mineTime = 5; 
    Testplayer myPlayer; 
	public float maxSpeed;

	float scale;

	public float points;

	Rigidbody2D rb2D;

	// Use this for initialization
	void Start () 
    {
        myPlayer = FindObjectOfType<Testplayer>();
		scale = Random.Range (.1f, 2);
		transform.localScale = new Vector3(scale,scale,scale);

		points = 10*scale;

		rb2D = GetComponent<Rigidbody2D>();

		rb2D.angularDrag = 2;

		maxSpeed = 2;

		rb2D.velocity = new Vector2(Random.Range(0,maxSpeed),Random.Range(0,maxSpeed));

		if(rb2D.velocity.magnitude > maxSpeed)
        {
			rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, maxSpeed);
        }
    }
	// Update is called once per frame
	void Update () 
    {
	
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
