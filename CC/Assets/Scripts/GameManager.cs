using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ExitLevel()
	{
		SceneManager.LoadScene("Menu");
	}

	public void Play()
	{
		SceneManager.LoadScene(1);
	}

    public void ExitGame()
    {
        Application.Quit();
    }
}
