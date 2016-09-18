using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour {
	
	float maxBoost;

	public float attachSpeed;
	float timeSinceBoost;

	public bool isBoosting;

	Testplayer player;
	
	
	
	// Use this for initialization
	void Start () 
	{
		maxBoost = 25;
		attachSpeed = 1.5f;
		timeSinceBoost = 0;

		player = GetComponent<Testplayer> ();
		
		isBoosting = false;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void FixedUpdate()
	{
		ShipBoost();
		Move();
	}
	
	void Move()
	{
		
		if (isBoosting == true) {
			if (timeSinceBoost > 1) {
				// Boost will cut off when timeSinceBoost == 1.5, so this formula ramps down velocity to just under 3 (maxSpeed).
				player.rb2D.velocity = new Vector2((maxBoost/Mathf.Pow(timeSinceBoost,6))*transform.up.x,(maxBoost/Mathf.Pow(timeSinceBoost,6))*transform.up.y);
			} else {
				// velocity is immediately set to maxBoost (25).
				player.rb2D.velocity = new Vector2(maxBoost*transform.up.x,maxBoost*transform.up.y);
			}
		}
	}

	void ShipBoost()
	{
		timeSinceBoost += Time.deltaTime;
		if (timeSinceBoost > 1.5) {
			isBoosting = false;
		}
		if (Input.GetKeyDown(KeyCode.Space) && timeSinceBoost > 3 && !player.isAttached) {
			Debug.Log ("isBoosting");
			timeSinceBoost = 0;
			isBoosting = true;
		}
	}
}
