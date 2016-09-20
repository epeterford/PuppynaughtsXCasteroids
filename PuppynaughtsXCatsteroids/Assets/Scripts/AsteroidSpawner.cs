using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	public float maxNum;
	public float currentNum;
	float timer;
	bool readySpawn;
	float spawnRadius;

	bool full;

	public GameObject[] asteroid;

	// Use this for initialization
	void Start () {
		timer = 1;
		StartCoroutine ("spawnTimer");
		maxNum = 12;
		currentNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentNum >= maxNum){
			full = true;
		}
		if(currentNum < maxNum && full){
			StartCoroutine("spawnTimer");
			full = false;
		}
			
	}

	IEnumerator spawnTimer(){
		yield return new WaitForSeconds (timer);

		timer = Random.Range (5, 10);

		spawn();

		if (!full) {
			StartCoroutine ("spawnTimer");
		}
	}

	void spawn(){

		Debug.Log ("Spawning asteroid");

		Vector3 spawnPos = (Random.Range (0f, 1f) > .5f) ? new Vector3 (1, Random.Range (0f, 1f), 0) : new Vector3 (Random.Range (0f, 1f), 1, 0);
		if (Random.Range (0f, 1f) > .5f) {
			spawnPos = (Random.Range (0f, 1f) > .5f) ? new Vector3 (0, Random.Range (0f, 1f), 0) : new Vector3 (Random.Range (0f, 1f), 0, 0);
		}
		spawnPos = Camera.main.ViewportToWorldPoint (spawnPos);

		GameObject rock = Instantiate (asteroid[Random.Range(0,asteroid.Length)], new Vector3(spawnPos.x,spawnPos.y,0), Quaternion.identity) as GameObject;

		//Vector3 target = new Vector3 (0, 0, 0);
		//rock.transform.LookAt (target);
		if (rock.transform.position.x < 0) {
			rock.transform.eulerAngles = new Vector3 (rock.transform.eulerAngles.x, rock.transform.eulerAngles.y, Mathf.Rad2Deg * Mathf.Atan (rock.transform.position.y / rock.transform.position.x) - 90);
		} else {
			rock.transform.eulerAngles = new Vector3 (rock.transform.eulerAngles.x, rock.transform.eulerAngles.y, Mathf.Rad2Deg * Mathf.Atan (rock.transform.position.y / rock.transform.position.x) + 90);

		}

		rock.GetComponent<Asteroid>().mom = GetComponent<AsteroidSpawner>();

		rock.transform.eulerAngles += new Vector3 (0, 0, Random.Range (-20, 20));

		currentNum++;


	}

}
