using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour {
	
	float maxBoost;

	float timeSinceBoost;

	public bool isBoosting;

	PlayerController player;
	
	
	
	// Use this for initialization
	void Start () 
	{
		maxBoost = 25;
		timeSinceBoost = 0;

        player = GetComponent<PlayerController> ();
		
		isBoosting = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeSinceBoost += Time.deltaTime;
		if (timeSinceBoost > 1f) {
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
			player.rb2D.velocity = new Vector2((maxBoost/Mathf.Pow(1+timeSinceBoost,2))*transform.up.x,(maxBoost/Mathf.Pow(1+timeSinceBoost,2))*transform.up.y);
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
