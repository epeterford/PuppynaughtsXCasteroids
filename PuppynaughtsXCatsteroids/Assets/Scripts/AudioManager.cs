using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour {

    public AudioSource efxSource;
    public AudioSource musicSource;

    public static AudioManager instance = null;

    public AudioClip[] sfxClips; 
    public AudioClip[] bgClilps;
	public AudioClip[] music;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    void Awake()
    {
        // Check if there is already an instance of AudioManager
        if(instance==null)
        {
            instance = this;
        }
        else if (instance !=this)
        {
            Destroy(gameObject);
        }

		musicSource.clip = music [0];
		musicSource.Play ();

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;

        efxSource.Play();
    }

    public void PlayRandomSFX()
    {
        int randomIndex = Random.Range(0, sfxClips.Length);

        float randomPitch = Random.Range (lowPitchRange, highPitchRange);

        efxSource.clip = sfxClips[randomIndex];
        efxSource.pitch = randomPitch;


        efxSource.Play();
    }

	public void PlayRandomExplosion(){
		int randomIndex = Random.Range(7, 12);

		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		efxSource.clip = sfxClips[randomIndex];
		efxSource.pitch = randomPitch;

		efxSource.Play ();
	}

	public void PlayRandomCollision(){
		int randomIndex = Random.Range(3, 7);

		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		efxSource.clip = sfxClips[randomIndex];
		efxSource.pitch = randomPitch;

		efxSource.Play ();
	}


	// Use this for initialization
	void Start ()
    {
        int randomIndex = Random.Range(0, bgClilps.Length);
       // musicSource.clip =  bgClilps[randomIndex];

        //musicSource.Play();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            PlayRandomSFX();
        }
	}
}
