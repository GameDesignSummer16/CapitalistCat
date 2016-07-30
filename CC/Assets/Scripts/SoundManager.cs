using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public AudioSource music;
	public AudioSource jumpSFX;
	public AudioSource pickupSFX;
	public AudioSource damageSFX;
	public AudioSource endLevelSFX;
	public AudioSource gameOverSFX;
	public AudioSource teleportSFX;

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
		else if (tag == "damage")
			damageSFX.Play ();
		else if (tag == "endLevel")
			endLevelSFX.Play ();
		else if (tag == "gameOver")
			gameOverSFX.Play ();
		else if (tag == "teleport")
			teleportSFX.Play ();
		else if (tag == "music")
			music.Play ();
	}

	public void stopSFX(string tag)
	{
		if (tag == "jump")
			jumpSFX.Stop ();
		else if (tag == "pickup")
			pickupSFX.Stop ();
		else if (tag == "damage")
			damageSFX.Stop ();
		else if (tag == "endLevel")
			endLevelSFX.Stop ();
		else if (tag == "gameOver")
			gameOverSFX.Stop ();
		else if (tag == "teleport")
			teleportSFX.Stop ();
		else if (tag == "music")
			music.Stop ();
	}
			
	
}
