<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject pauseButton;
	public GameObject pauseMenu;
	public GameObject screenFader;

	private bool isPaused;


	// Use this for initialization
	void Start () 
	{
		isPaused = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void restartLevel()
	{

		Application.LoadLevel (Application.loadedLevel);
	//	screenFader.gameObject.GetComponent<FadeInOut> ().EndScene (1);
		Debug.Log ("restart");

	}
	public void play()
	{
		screenFader.gameObject.GetComponent<FadeInOut> ().sceneStarting = true;
		Application.LoadLevel (1);
		Debug.Log ("Play");
	}

	public void mainMenu()
	{
		//screenFader.gameObject.GetComponent<FadeInOut> ().EndScene (1);
		Application.LoadLevel (0);
		Debug.Log ("Main Menu");
	}

	public void nextLevel()
	{
		//screenFader.gameObject.GetComponent<FadeInOut> ().EndScene (1);
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void restartFromCheckpoint()
	{
		Application.LoadLevel (Application.loadedLevel);
		Debug.Log ("Checkpoint Reset");
	}

	public void exitGame()
	{
		Application.Quit ();
	}

	public void pauseGame()
	{

			if (isPaused == true) 
			{
				pauseButton.SetActive(false);
				pauseMenu.SetActive(true);
				Time.timeScale = 0f;
				isPaused = false;
			} else {
				pauseButton.SetActive(true);
				pauseMenu.SetActive(false);
				Time.timeScale = 1f;
				isPaused = true;
			}
			
	}

	public void fadeToBlack()
	{
		screenFader.GetComponent<FadeInOut> ().FadeToBlack ();
	}

	public void fadeToClear()
	{
		screenFader.GetComponent<FadeInOut> ().FadeToClear ();
	}

	public void toggleSceneStarting()
	{
		screenFader.GetComponent<FadeInOut> ().sceneStarting = true;
	}

	public void getHighScores()
	{

	}

}
=======
﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject pauseButton;
	public GameObject pauseMenu;

	private bool isPaused;


	// Use this for initialization
	void Start () 
	{
		isPaused = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void restartLevel()
	{

		Application.LoadLevel (Application.loadedLevel);
		Debug.Log ("restart");

	}
	public void play()
	{

		Application.LoadLevel (1);
		Debug.Log ("Play");
	}

	public void mainMenu()
	{
		Application.LoadLevel (0);
		Debug.Log ("Main Menu");
	}

	public void nextLevel()
	{
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void restartFromCheckpoint()
	{
		Application.LoadLevel (Application.loadedLevel);
		Debug.Log ("Checkpoint Reset");
	}

	public void exitGame()
	{
		Application.Quit ();
	}

	public void pauseGame()
	{

			if (isPaused == true) 
			{
				pauseButton.SetActive(false);
				pauseMenu.SetActive(true);
				Time.timeScale = 0f;
				isPaused = false;
			} else {
				pauseButton.SetActive(true);
				pauseMenu.SetActive(false);
				Time.timeScale = 1f;
				isPaused = true;
			}
			
	}

}
>>>>>>> origin/master
