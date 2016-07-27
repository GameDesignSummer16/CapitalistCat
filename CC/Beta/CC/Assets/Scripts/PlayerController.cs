/*
* Author: Arash Tadjiki
* Date - 17/7/2016
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
* - Teleporting
* - Sound effect/Music control
* 
* Alpha:
* - Added acceleration and speed regulation (in progress)
* - Moving platforms can now be fallen through 
* as well as moving enemy types
* - Reverse direction on trigger added for teleport objects
* - Score calculation modified (now based on pickups rather than speed)
* 
* 
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
	public float pickUpBoost = 1.5f;
	public int runMultiplier = 2;
	public int damageSlowdown = 1;
	public bool infiniteRunnerMode = true;

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
	private float fallSpeed;
	private float walkSpeed;
	private float currentSpeed;
	private bool positiveVelocity;
	private float acceleration;


	//plummet mode
	private float baseScore;
	private float bonusScore;

	private ArrayList pickups;

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

		pickups = new ArrayList ();

		positiveVelocity = true;
		acceleration = 1;

	}

	// Update is called once per frame
	void Update ()
	{
		//check for pause input
		if (Input.GetKeyDown (KeyCode.P) && playerControl) {
			pauseGame ();
		}

		//only allow input if game is in play
		if (playerControl == true) {
			updateProgressBar ();
			Vector3 input = playerInput ();
			_controller.move (input * Time.deltaTime);

			if (infiniteRunnerMode == true) {
				currentSpeed = (int)Mathf.Abs (input.x);
				speedText.text = "Speed: " + currentSpeed; 
			} else
				speedText.text = "Score: " + (int)(baseScore + bonusScore);
		}
	}

	public Vector3 playerInput ()
	{
		Vector3 velocity = _controller.velocity;

		//check if player is standing on moving platform
		if (infiniteRunnerMode)
			CheckMovingPlatform ();

		//if false, allow the player to move normally
		if (infiniteRunnerMode == false) {
			
			velocity.y = -fallSpeed;

			//directional inputs
			if (Input.GetAxis ("Horizontal") < 0)
				velocity.x = -walkSpeed * runMultiplier;
			

			if (Input.GetAxis ("Horizontal") > 0)
				velocity.x = walkSpeed * runMultiplier;
			
		} else { //if infinite mode is active:
				
			velocity.x = walkSpeed * runMultiplier;

			if (Input.GetAxis ("Jump") > 0 && _controller.isGrounded) {
				//play sound effect
				soundManager.GetComponent<SoundManager> ().playSFX ("jump");

				//neutralize momentum
				if (walkSpeed > minimumWalkSpeed)
					walkSpeed -= pickUpBoost;

				if (Input.GetKey (KeyCode.LeftShift))
					velocity.y = Mathf.Sqrt (4f * jumpHeight * -gravity);
				else
					velocity.y = Mathf.Sqrt (2f * jumpHeight * -gravity);
			}
				
		}


		currentSpeed = velocity.x;
		RegulateSpeed (); //speed up or slow down player
		velocity.y += gravity * Time.deltaTime;
		gameObject.GetComponent<CharacterController2D> ().move (velocity * Time.deltaTime);

		return velocity;
		
	}
		
	void OnTriggerEnter2D (Collider2D col)
	{

		if (col.tag == "KillZ") {
			PlayerDeath (); //displays player death screen
		} else if (col.tag == "Damaging") {
			PlayerDamage (damageSlowdown, col); //when the player collides with an enemy 
		} else if (col.tag == "Teleport") {
			DoTeleport (col); 
		} else if (col.tag == "Finish") {
			EndOfLevel ();
		} else if (col.tag == "Pickup") {
			DoPickup (col);
		}
	}

	/*
	 * Applies damage slowdown to player speed
	 */
	private void PlayerDamage (int damage, Collider2D col)
	{
				
		walkSpeed = 4;
		if (walkSpeed == 0) {
			PlayerDeath ();
		}
	}

	/*
	 * Freezes game and displays Game Over Panel
	 */
	private void PlayerDeath ()
	{
		freezeGame ();
		gameOverPanel.SetActive (true);
		soundManager.GetComponent<SoundManager> ().stopSFX ("music");
		soundManager.GetComponent<SoundManager> ().playSFX ("gameOver");
	}

	private void CheckMovingPlatform ()
	{
		if (_controller.isGrounded && _controller.ground != null && _controller.ground.tag == "Moving Platform")
			this.transform.parent = _controller.ground.transform;
		else {
			if (this.transform.parent != null)
				transform.parent = null;
		}
	}

	/*
	 * Level Progress Bar fills up according to player
	 * distance from finish point. Resets if player loops
	 * level.
	 */
	private void updateProgressBar ()
	{
		float currentPosition = transform.position.x - startPosition;
		progressBar.fillAmount = (currentPosition / levelDistance);
	}

	private void pauseGame ()
	{
		gameManager.GetComponent<GameManager> ().pauseGame ();
	}

	private void EndOfLevel ()
	{
		freezeGame ();
		UIPanel.SetActive (false);
		baseScore = currentSpeed;
		scoreText.text = "Score: " + (int)Mathf.Abs (baseScore + bonusScore);
		endlevelPanel.SetActive (true);
		soundManager.GetComponent<SoundManager> ().stopSFX ("music");
		soundManager.GetComponent<SoundManager> ().playSFX ("endLevel");
	}

	private void freezeGame ()
	{
		gameCamera.GetComponent<CameraFollow2D> ().stopCameraFollow ();
		Time.timeScale = 0;
		playerControl = false;
	}

	private void unFreezeGame ()
	{
		gameCamera.GetComponent<CameraFollow2D> ().startCameraFollow (this.gameObject);
		Time.timeScale = 1;
		playerControl = true;
	}

	private void TeleportTo (Transform endPoint, int count)
	{
		
		freezeGame ();
		this.transform.position = endPoint.position;
		ReActivatePickups ();
		unFreezeGame ();
	}

	/*
	 * When the player teleports somewhere, all pickups are re-activated
	 */
	private void ReActivatePickups ()
	{
		ArrayList temp = new ArrayList ();

		foreach (GameObject g in pickups) {
			g.SetActive (true);
			temp.Add (g);
		}

		foreach (GameObject g in temp) {
			pickups.Remove (g);
		}

	}

	/*
	 * Called when the player collides with 
	 * a pickup object
	 */
	private void DoPickup (Collider2D col)
	{
		soundManager.GetComponent<SoundManager> ().playSFX ("pickup");

		/* if the game is in runner mode, 
		 * pickups contribute to speed.
		 * Otherwise they add to score while
		 * the player is falling
		 */
		if (infiniteRunnerMode == true) {
			if (positiveVelocity)
				walkSpeed += pickUpBoost;
			else
				walkSpeed -= pickUpBoost;
		} else {
			if (fallSpeed >= baseScore)
				fallSpeed -= pickUpBoost;
			bonusScore += pickUpBoost;
		}

		//deactivate pickup 
		col.gameObject.SetActive (false);
		if (!(pickups.Contains (col.gameObject)))
			pickups.Add (col.gameObject);
	}

	/*
	 * Manages teleportation between parts of the level
	 * Checks if the teleport box should be destroyed, 
	 * if the game should enter plummet mode, 
	 * or if the player should reverse directions
	 */
	private void DoTeleport (Collider2D col)
	{

		soundManager.GetComponent<SoundManager> ().playSFX ("teleport");

		if (col.gameObject.activeSelf == true)
			TeleportTo (col.gameObject.GetComponent<Teleport> ().getEndTransform (), 100000);

		if (col.gameObject.GetComponent<Teleport> ().plummetOnTrigger == true)
			TogglePlummet ();
		else if (col.gameObject.GetComponent<Teleport> ().destroyOnTrigger == true)
			col.gameObject.SetActive (false);
		else if (col.gameObject.GetComponent<Teleport> ().reverseOnTrigger == true) {
			walkSpeed = walkSpeed * -1; 
			Flip ();
		}
	}

	// Flip sprite / animation over the x-axis
	private void Flip ()
	{
		positiveVelocity = !positiveVelocity;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

	}

	/*
	 * Puts the game into plummet mode. 
	 * The player can now move left and right 
	 * while falling.
	 */
	private void TogglePlummet ()
	{
		progressBar.gameObject.SetActive (false);

		baseScore = currentSpeed;
		fallSpeed = baseScore;
		bonusScore = 0;
		infiniteRunnerMode = false;

	}

	/*
	 * Simulates "acceleration". 
	 * At low speeds, the player speeds up quickly, 
	 * but getting to maximum speed takes more time.
	 * If the player is going faster than maximum, 
	 * they are gradually slowed back down.
	 * Each speed bracket applies a different coefficient
	 * of acceleration to the walkspeed. 
	 */
	private void RegulateSpeed ()
	{
		if (currentSpeed < 4) {
			acceleration = 1.1f;
		} else if (currentSpeed > 4 && currentSpeed < 5) {
			acceleration = 1.01f;
		} else if (currentSpeed > 5 && currentSpeed < 6) {
			acceleration = 1.001f;
		} else if (currentSpeed > 6 && currentSpeed < 7) {
			acceleration = 1.0001f;
		} else if (currentSpeed > 7 && currentSpeed < 8) {
			acceleration = 1.00001f;
		} else if (currentSpeed > 8) {
			acceleration = 0.99f;
		}

		walkSpeed = walkSpeed * acceleration;
		
	}


}
