using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour {

	public GameObject gameCamera;
    public GameObject gameOverPanel;

	public float jumpHeight = 2;
	public float walkSpeed = 3;
	public float gravity = -35;

	public int health = 100;

	private CharacterController2D _controller;
    //private AnimationController2D _animator;
    //Playzone is the screen area, the area the player cannot exit.
    private Vector3 playZone;

	private int currentHealth = 0;
	private bool playerControl = true;
    private float half;
	// Use this for initialization
	void Start () {
        //initialize _controller component
		_controller = gameObject.GetComponent<CharacterController2D>();
        //playZone initialization
        playZone = gameCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
        //Half is the the middle x point of the sprite
        half = gameObject.GetComponent<Renderer>().bounds.size.x / 2;
        //_animator = gameObject.GetComponent<AnimationController2D>();

        //gameCamera.GetComponent<CameraFollow2D>().startCameraFollow(this.gameObject);

        currentHealth = health;
	}
	
	// Update is called once per frame
	void Update ()
	{
        playZone = Camera.main.WorldToViewportPoint(transform.position);
        playZone.x = Mathf.Clamp(playZone.x, 0.05f, 0.95f);
        playZone.y = Mathf.Clamp01(playZone.y);
        transform.position = Camera.main.ViewportToWorldPoint(playZone);
        if (playerControl)
		{
			Vector3 velocity = PlayerInput();
			_controller.move(velocity * Time.deltaTime);
		}

	}

    private Vector3 PlayerInput()
	{
		Vector3 velocity = _controller.velocity;

		if (_controller.isGrounded && _controller.ground != null && _controller.ground.tag == "MovingPlatform")
		{
			this.transform.parent = _controller.ground.transform;
		}
		else
		{
			if (this.transform.parent != null)
			{
				transform.parent = null;
			}
		}

		if (Input.GetAxis("Horizontal") < 0)
		{
                      
            velocity.x = walkSpeed * -1;
            if (_controller.isGrounded)
            {
                //Play walking anim
                //_animator.setAnimation("RUN");
            } 
            //_animator.setFacing("Left");
            	

		}
		else if (Input.GetAxis("Horizontal") > 0)
		{
			velocity.x = walkSpeed;
			if (_controller.isGrounded)
			{
				//Play walking anim
				//_animator.setAnimation("RUN");
			}
			//_animator.setFacing("Right");
		}
		else
		{
			//play idle anim
			//_animator.setAnimation("IDLE");
		}

		if (Input.GetAxis("Jump") > 0 && _controller.isGrounded)
		{
			velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
			//play jump anim
			//_animator.setAnimation("JUMP");
		}

		velocity.x *= .92f;

		velocity.y += gravity * Time.deltaTime;

		return velocity;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//Check what we're hitting.
		if (col.tag == "KillZ")
		{
			PlayerFallDeath();
		}
		else if(col.tag == "Damaging")
		{
			PlayerDamage(10);
		}
	}

	private void PlayerDamage (int damage)
	{
		currentHealth -= damage;

		float normalizedHealth = (float)currentHealth / (float)health;

		//healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedHealth*256,32);

		if (currentHealth <= 0)
		{
			currentHealth = 0;
			PlayerDeath();
		}
	}

	private void PlayerDeath()
	{
		//_animator.setAnimation("DEATH");
		playerControl = false;
        gameOverPanel.SetActive(true);
	}

	private void PlayerFallDeath()
	{
		currentHealth = 0;
		//healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 32);
		Debug.Log(currentHealth);

		//gameCamera.GetComponent<CameraFollow2D>().stopCameraFollow();

        gameOverPanel.SetActive(true);
    }
}
