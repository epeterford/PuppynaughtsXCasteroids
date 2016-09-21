using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	float maxNum = 12; // Max number of asteroids allowed at one time
	public float currentNum = 0; // Current number of asteroids on the field
	float timer = 1;
	bool readySpawn;
	float spawnRadius;
    bool spawningAsteroid = true;
	public GameObject[] asteroid;

	// Use this for initialization
	void Start () 
    {
        currentNum = 0;
		StartCoroutine ("SpawnTimer");
	}
	
	// Update is called once per frame
	void Update () 
    {
        Debug.Log("Current # of Asteroids: " + currentNum.ToString());

        if(! IsFieldFull() && !spawningAsteroid )
        {
            StartCoroutine("SpawnTimer");
        }
			
	}

	IEnumerator SpawnTimer()
    {
        spawningAsteroid = true;
		yield return new WaitForSeconds (timer);

		timer = Random.Range (5, 10);

		SpawnAsteroid();
        spawningAsteroid = false;
	}

    bool IsFieldFull()
    {
        return currentNum>=maxNum; 

    }
	void SpawnAsteroid()
    {
		Vector3 spawnPos = (Random.Range (0f, 1f) > .5f) ? new Vector3 (1, Random.Range (0f, 1f), 0) : new Vector3 (Random.Range (0f, 1f), 1, 0);
		if (Random.Range (0f, 1f) > .5f) 
        {
			spawnPos = (Random.Range (0f, 1f) > .5f) ? new Vector3 (0, Random.Range (0f, 1f), 0) : new Vector3 (Random.Range (0f, 1f), 0, 0);
		}
		spawnPos = Camera.main.ViewportToWorldPoint (spawnPos);

		GameObject rock = Instantiate (asteroid[Random.Range(0,asteroid.Length)], new Vector3(spawnPos.x,spawnPos.y,0), Quaternion.identity) as GameObject;

		if (rock.transform.position.x < 0) 
        {
			rock.transform.eulerAngles = new Vector3 (rock.transform.eulerAngles.x, rock.transform.eulerAngles.y, Mathf.Rad2Deg * Mathf.Atan (rock.transform.position.y / rock.transform.position.x) - 90);
		} 
        else
        {
			rock.transform.eulerAngles = new Vector3 (rock.transform.eulerAngles.x, rock.transform.eulerAngles.y, Mathf.Rad2Deg * Mathf.Atan (rock.transform.position.y / rock.transform.position.x) + 90);
		}
		rock.GetComponent<Asteroid>().mom = GetComponent<AsteroidSpawner>();

		rock.transform.eulerAngles += new Vector3 (0, 0, Random.Range (-20, 20));

		currentNum++;
	}

}
