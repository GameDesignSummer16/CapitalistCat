/*
* Author: Arash Tadjiki
* Date - 12/6/2016
* 
* Prototype Build:
* This script, based on the tutorial controller, 
* controls the core mechanics of the game, allowing the 
* player to move in either infinite runner mode
* or in normal platformer mode. Several public variables
* have been added: a run multiplier, and booleans
* for toggling infinite runner mode and walking.
* 
* Feature Complete:
* - Added Features:
* - Pausing, End of Screen, 
*/
using UnityEngine;
using UnityEngine.UI;
using Prime31;
using System.Collections;

public class PlayerController : MonoBehaviour 
{

	public float gravity = -35;
	public float jumpHeight = 10;
	public float minimumWalkSpeed = 3;
	public float friction = 0.96f;
	public float pickUpMultiplier = 1.5f;
	public int runMultiplier = 2;
	public bool infiniteRunnerMode = true;
	public bool enableWalking = true;

	public GameObject gameManager;
	public GameObject soundManager;
	public GameObject gameCamera;
	public GameObject endlevelPanel;
	public GameObject gameOverPanel;
	public GameObject UIPanel;
	public GameObject levelFinish;
	public Text speedText;
	public Text scoreText;
	public Image progressBar;

	private CharacterController2D _controller;
	private AnimationController2D _animator;
	private bool playerControl = true;
	private float startPosition;
	private float endPosition;
	private float levelDistance;
	private float walkSpeed;
	private int currentSpeed;

	// Use this for initialization
	void Start () 
	{
		_controller = gameObject.GetComponent<CharacterController2D> ();
		_animator = gameObject.GetComponent<AnimationController2D> ();

		gameCamera.GetComponent<CameraFollow2D> ().startCameraFollow (this.gameObject);
		startPosition = transform.position.x;
		endPosition = levelFinish.transform.position.x;
		levelDistance = endPosition - startPosition;
		Time.timeScale = 1;
		walkSpeed = minimumWalkSpeed;
	}

	// Update is called once per frame
	void Update () 
	{
		//check for pause input
		if (Input.GetKeyDown (KeyCode.P) && playerControl) 
		{
			pauseGame ();
		}
			
		if (playerControl == true) {
			updateProgressBar ();
			Vector3 input = playerInput ();
			_controller.move (input * Time.deltaTime);
			currentSpeed = (int)input.x;
			speedText.text = "Speed: " + currentSpeed;
		}
	}

	public Vector3 playerInput()
	{
		Vector3 velocity = _controller.velocity;

		//check if player is standing on moving platform
		CheckMovingPlatform();

		//if false, allow the player to move normally
		if (infiniteRunnerMode == false) 
		{
			velocity.x = 0;

			//directional inputs
			if (Input.GetAxis ("Horizontal") < 0) 
			{

				if (enableWalking) 
				{
					if (_controller.isGrounded && Input.GetKey (KeyCode.LeftShift)) 
					{
						velocity.x = -walkSpeed * runMultiplier;
					} 
					else if (_controller.isGrounded) 
					{
						velocity.x = -walkSpeed;
					}
				} else 
				{
					velocity.x = -walkSpeed * runMultiplier;
				}
			} 
			else if (Input.GetAxis ("Horizontal") > 0) 
			{

				if (enableWalking) {
					if (_controller.isGrounded && Input.GetKey (KeyCode.LeftShift)) {
						velocity.x = walkSpeed * runMultiplier;
						_animator.setAnimation ("RunPlayer");
					} else if (_controller.isGrounded) {
						velocity.x = walkSpeed;
						_animator.setAnimation ("WalkPlayer");
					}
				} else 
				{
					velocity.x = walkSpeed * runMultiplier;
					_animator.setAnimation ("RunPlayer");
				}

				_animator.setFacing ("Right");

			} 
			else 
			{
				_animator.setAnimation("IdlePlayer");
			}
		}
		else //if infinite mode is active:
		{

			if (enableWalking) {
				if (_controller.isGrounded && Input.GetKey (KeyCode.LeftShift)) {
					velocity.x = walkSpeed * runMultiplier;
				} else if (_controller.isGrounded) {
					velocity.x = walkSpeed;
				}
			} else 
			{
				velocity.x = walkSpeed * runMultiplier;
			}

		}

		if (Input.GetAxis ("Jump") > 0 && _controller.isGrounded) 
		{
			//play sound effect
			soundManager.GetComponent<SoundManager> ().playSFX("jump");

			//neutralize momentum
			if(walkSpeed > minimumWalkSpeed)
				walkSpeed -= pickUpMultiplier;

			if (Input.GetKey (KeyCode.LeftShift)) {
				velocity.y = Mathf.Sqrt (4f * jumpHeight * -gravity);	
			} 
			else 
			{
				velocity.y = Mathf.Sqrt (2f * jumpHeight * -gravity);
			}

			//play jump animation

		}

		velocity.x *= friction;

		velocity.y += gravity * Time.deltaTime;
		gameObject.GetComponent<CharacterController2D>().move(velocity * Time.deltaTime);

		return velocity;
	}

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.tag == "KillZ") {
			PlayerDeath ();
		} else if (col.tag == "Damaging") {
			PlayerDamage (1);
		} else if (col.tag == "Finish") {
			EndOfLevel ();
		} else if (col.tag == "Pickup") {
			soundManager.GetComponent<SoundManager> ().playSFX("pickup");
			walkSpeed += pickUpMultiplier;
			col.gameObject.SetActive (false);


		}
	}
		
	private void PlayerDamage(int damage)
	{
		walkSpeed -= damage;
		if (walkSpeed == 0) 
		{
			PlayerDeath ();
		}
	}

	private void PlayerDeath()
	{
		freezeGame();
		gameOverPanel.SetActive (true);
	}

	private void CheckMovingPlatform()
	{
		if (_controller.isGrounded && _controller.ground != null && _controller.ground.tag == "Moving Platform")
			this.transform.parent = _controller.ground.transform;
		else {
			if (this.transform.parent != null)
				transform.parent = null;
		}
	}
	private void updateProgressBar()
	{
		float currentPosition = transform.position.x - startPosition;
		progressBar.fillAmount = (currentPosition / levelDistance);
	}

	private void pauseGame()
	{
		gameManager.GetComponent<GameManager> ().pauseGame ();
	}

	private void EndOfLevel()
	{
		freezeGame ();
		UIPanel.SetActive (false);
		scoreText.text = "Score: " + currentSpeed;
		endlevelPanel.SetActive (true);
	}

	private void freezeGame()
	{
		gameCamera.GetComponent<CameraFollow2D> ().stopCameraFollow ();
		Time.timeScale = 0;
		playerControl = false;
	}
}
