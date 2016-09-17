﻿using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    float mineTime = 0; 
    Testplayer myPlayer; 
	public float maxSpeed;
	public float startSpeed;

	float scale;

	public float points;

	Rigidbody2D rb2D;

	// Use this for initialization
	void Start () 
    {
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
        Destroy(this.gameObject);

    }
}
