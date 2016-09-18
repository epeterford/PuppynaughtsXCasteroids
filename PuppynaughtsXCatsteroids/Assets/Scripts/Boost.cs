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
		timeSinceBoost += Time.deltaTime;
		if (timeSinceBoost > 1.5f) {
			isBoosting = false;
		}
	}
	
	void FixedUpdate()
	{
		Move();
	}
	
	void Move()
	{
		
		if (isBoosting == true) {
			// Boost will cut off when timeSinceBoost == 1.5, so this formula ramps down velocity to just under 3 (maxSpeed).
			player.rb2D.velocity = new Vector2((maxBoost/Mathf.Pow(2+timeSinceBoost,2))*transform.up.x,(maxBoost/Mathf.Pow(2+timeSinceBoost,2))*transform.up.y);
		}
	}

	public void ShipBoost()
	{
		if (timeSinceBoost > 3) {
			Debug.Log ("isBoosting");
			timeSinceBoost = 0;
			isBoosting = true;
		}
	}
}
