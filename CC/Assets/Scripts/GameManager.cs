using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


	public GameObject lastCheckpoint;

	// Use this for initialization
	void Start () 
	{
	
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

	public void restartFromCheckpoint()
	{
		
		Application.LoadLevel (Application.loadedLevel);
		Debug.Log ("Checkpoint Reset");
	}

	public void exitGame()
	{
		Application.Quit ();
	}
}
