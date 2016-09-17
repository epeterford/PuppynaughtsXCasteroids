using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	public float maxNum;
	float timer;
	bool readySpawn;
	float spawnRadius;
	Vector3 center;

	// Use this for initialization
	void Start () {
		timer = 0;
		center = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator spawnTimer(){
		yield return new WaitForSeconds (timer);

		timer = Random.Range (5, 15);


	}

	void spawn(){
		//Vector2vect
		//Vector3 spawnPos = new Vector3(
			

	}
}
