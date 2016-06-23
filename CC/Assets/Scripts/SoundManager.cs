using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public AudioSource music;
	public AudioSource jumpSFX;
	public AudioSource pickupSFX;

	private bool musicMuted;

	// Use this for initialization
	void Start () 
	{
		musicMuted = false;
		music.Play ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void toggleMusic()
	{
		if (musicMuted == true) {
			music.Play ();
			musicMuted = false;
		} else {
			music.Pause();
			musicMuted = true;
		}	
	}

	public void playSFX(string tag)
	{
		if (tag == "jump")
			jumpSFX.Play ();
		else if (tag == "pickup")
			pickupSFX.Play ();
	}
			
	
}
