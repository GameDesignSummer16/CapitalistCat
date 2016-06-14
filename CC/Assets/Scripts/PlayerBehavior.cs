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
*/
using UnityEngine;
using UnityEngine.UI;
using Prime31;
using System.Collections;

public class PlayerBehavior : MonoBehaviour 
{

	public float gravity = -35;
	public float jumpHeight = 10;
	public float walkSpeed = 3;
	public float friction = 0.96f;
	public int health = 100;
	public int runMultiplier = 2;
	public bool infiniteRunnerMode = true;
	public bool enableWalking = true;

	public GameObject startCheckpoint;
	public GameObject healthBar;
	public GameObject gameCamera;
	public GameObject gameOverPanel;

	private CharacterController2D _controller;
	private AnimationController2D _animator;
	private int _currentHealth = 0;
	private bool playerControl = true;




	// Use this for initialization
	void Start () 
	{

		if (!(ReferenceEquals (startCheckpoint, null)))
			transform.position = startCheckpoint.transform.position;

		_controller = gameObject.GetComponent<CharacterController2D> ();
		_animator = gameObject.GetComponent<AnimationController2D> ();
		_currentHealth = health;

		gameCamera.GetComponent<CameraFollow2D> ().startCameraFollow (this.gameObject);
		

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerControl == true) 
		{
			_controller.move (playerInput () * Time.deltaTime);
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
						_animator.setAnimation ("RunPlayer");
					} 
					else if (_controller.isGrounded) 
					{
						velocity.x = -walkSpeed;
						_animator.setAnimation ("WalkPlayer");
					}
				} else 
				{
					velocity.x = -walkSpeed * runMultiplier;
					_animator.setAnimation ("RunPlayer");
				}

				_animator.setFacing ("Left");

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




		if (Input.GetAxis ("Jump") > 0 && _controller.isGrounded) 
		{
			if (Input.GetKey (KeyCode.LeftShift)) {
				velocity.y = Mathf.Sqrt (4f * jumpHeight * -gravity);	
			} 
			else 
			{
				//Debug.log ("Space bar pressed);
				velocity.y = Mathf.Sqrt (2f * jumpHeight * -gravity);
			}

			//play jump animation
			_animator.setAnimation("JumpPlayer");
		}

		velocity.x *= friction;

		velocity.y += gravity * Time.deltaTime;
		gameObject.GetComponent<CharacterController2D>().move(velocity * Time.deltaTime);

		return velocity;
	}

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.tag == "KillZ") {
			PlayerFallDeath ();
		} else if (col.tag == "Damaging") {
			PlayerDamage (30);
		} else if (col.tag == "Finish") {
			Application.LoadLevel (0);
		} else if (col.tag == "Checkpoint") {

			//attaches player to the most recently
			//touched checkpoint
			Debug.Log(col.gameObject.name);
			startCheckpoint = col.gameObject;
	
		}
	}

	private void PlayerFallDeath()
	{
		
		_currentHealth = 0;
		float normalizedHealth = (float) _currentHealth / (float) health;
		healthBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (normalizedHealth * 256, 32);
		gameCamera.GetComponent<CameraFollow2D> ().stopCameraFollow();

		gameOverPanel.SetActive (true);
	}

	private void PlayerDamage(int damage)
	{
		
		_currentHealth -= damage;
		float normalizedHealth = (float) _currentHealth / (float) health;
		healthBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (normalizedHealth * 256, 32);

		if (_currentHealth <= 0) 
		{
			PlayerDeath ();
		}

	}

	private void PlayerDeath()
	{
		playerControl = false;
		_animator.setAnimation ("DeathPlayer");

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
}
