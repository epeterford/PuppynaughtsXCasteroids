﻿using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    float mineTime = 5; 
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
		scale = Random.Range (.1f, 2);
		transform.localScale = new Vector3(scale,scale,scale);

		rb2D = GetComponent<Rigidbody2D>();

		rb2D.mass = scale;

		points = 10*scale;

		rb2D.angularDrag = 2;

		maxSpeed = 2;

		startSpeed = Random.Range (.2f, maxSpeed);

		rb2D.AddForce (maxSpeed * 20 * transform.up);

<<<<<<< HEAD
		if(rb2D.velocity.magnitude > maxSpeed)
        {
			rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, maxSpeed);
        }
    }
=======

<<<<<<< HEAD
=======
		if(rb2D.velocity.magnitude > maxSpeed){
			rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, maxSpeed);
		}
>>>>>>> fbd04dec550843a228ecfc7688601b576e180557
>>>>>>> ccebb59211645ffb64dcedb75c13f3986ade0a1e
	}
	
>>>>>>> 14337314a4d3b516abae1b6adc400db30127db29
	// Update is called once per frame
	void Update () 
    {
	
	}

<<<<<<< HEAD
	void FixedUpdate(){
		if(rb2D.velocity.magnitude > maxSpeed){
			rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, maxSpeed);
		}
	}
		
=======
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
>>>>>>> ccebb59211645ffb64dcedb75c13f3986ade0a1e
}
