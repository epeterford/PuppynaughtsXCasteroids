using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	public float maxNum;
	float timer;
	bool readySpawn;
	float spawnRadius;
	Vector3 center;

	public GameObject asteroid;

	// Use this for initialization
	void Start () {
		timer = 1;
		center = new Vector3 (0, 0, 0);
		StartCoroutine ("spawnTimer");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator spawnTimer(){
		yield return new WaitForSeconds (timer);

		timer = Random.Range (5, 10);

		spawn();

		StartCoroutine ("spawnTimer");
	}

	void spawn(){

		Debug.Log ("Spawning asteroid");

		Vector3 spawnPos = (Random.Range (0f, 1f) > .5f) ? new Vector3 (1, Random.Range (0f, 1f), 0) : new Vector3 (Random.Range (0f, 1f), 1, 0);
		if (Random.Range (0f, 1f) > .5f) {
			spawnPos = (Random.Range (0f, 1f) > .5f) ? new Vector3 (0, Random.Range (0f, 1f), 0) : new Vector3 (Random.Range (0f, 1f), 0, 0);
		}
		spawnPos = Camera.main.ViewportToWorldPoint (spawnPos);

		GameObject rock = Instantiate (asteroid, new Vector3(spawnPos.x,spawnPos.y,0), Quaternion.identity) as GameObject;

		//Vector3 target = new Vector3 (0, 0, 0);
		//rock.transform.LookAt (target);
		if (rock.transform.position.x < 0) {
			rock.transform.eulerAngles = new Vector3 (rock.transform.eulerAngles.x, rock.transform.eulerAngles.y, Mathf.Rad2Deg * Mathf.Atan (rock.transform.position.y / rock.transform.position.x) - 90);
		} else {
			rock.transform.eulerAngles = new Vector3 (rock.transform.eulerAngles.x, rock.transform.eulerAngles.y, Mathf.Rad2Deg * Mathf.Atan (rock.transform.position.y / rock.transform.position.x) + 90);

		}

		rock.transform.eulerAngles += new Vector3 (0, 0, Random.Range (-20, 20));


	}
}
