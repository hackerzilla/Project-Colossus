using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//TODO rename initialJumpHeight to initialJumpForce
	public float speed, speedMidAir, speedRunning, initialJumpForce, maxHoldTime, forcePerFrame;
	public bool canJump, jumped, midAir, running;

	private float timeAtJump;

	private GameObject player;
	private Animator anim;
	private Rigidbody2D rb;
	private SpriteRenderer sr;


	void Start () {
		player = this.gameObject;
		anim = player.GetComponent<Animator> ();
		rb = player.GetComponent<Rigidbody2D> ();
		sr = player.GetComponent<SpriteRenderer> ();
		canJump = true;
		jumped = false;
		midAir = false;
		running = false;
	}

	void Update ()
	{
		//take player input

		if (Input.GetKey (KeyCode.LeftShift)) {
			//Sets the running bool to true (and false) in this script and in the animator
			anim.SetBool ("isRunning", true);
			running = true;
		} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
			anim.SetBool ("isRunning", false);
			running = false;
		}

		if (Input.GetKey (KeyCode.D)) {
			//Move right
			sr.flipX = false;
			anim.SetBool ("isWalking", true);
			AddForceToPlayer (false);
		}
		if (Input.GetKey (KeyCode.A)) {
			//Move left
			sr.flipX = true;
			anim.SetBool ("isWalking", true);
			AddForceToPlayer (true);
		}
			
		if (Input.GetKeyDown (KeyCode.Space) && canJump) {
			//Jump
			Jump ();
		}

		if (Input.GetKey (KeyCode.Space) && jumped) {
			//Add velocity the longer you hold space
			Jumping();
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			timeAtJump = 0f;
			jumped = false;
		}

		if (!Input.anyKey) {
			anim.SetBool ("isWalking", false);
		}
	}

	public void Respawn () {
		//resets the player's transform to the origin

		this.transform.position = new Vector2 (0,10);
	}

	void Jump () {
		//Adds an initial force upwards to the player's rigidbody2d
		canJump = false;
		jumped = true;
		anim.SetTrigger ("jumpTrigger");
		rb.AddForce (Vector2.up * initialJumpForce);
		timeAtJump = Time.time;
	}

	void Jumping () {
		//Adds more rigidbody2d.force in the upwards direction the longer spacebar is held down
		if (timeAtJump > 0 && Time.time < timeAtJump + maxHoldTime) {
			//In other words: if timeAtJump has been set (meaning space hasn't been lifted because getkeyup resests timeAtJump)
			//and its still under the time limit specified by maxHoldTime
			//then do this...
			rb.AddForce (new Vector2(0, forcePerFrame));
			//Debug.Log ("Jumping...");
		}
	}

	void AddForceToPlayer (bool left) {
		//left = if the player is moving left

		int directionFlipper = 1;

		if (left) {
			directionFlipper = -1;
		}

		if (midAir) {
			rb.AddForce (Vector2.right * directionFlipper * speedMidAir * Time.deltaTime * 100);
		} else if (running) {
			rb.AddForce (Vector2.right * directionFlipper * speedRunning * Time.deltaTime * 100);
		} else {
			rb.AddForce (Vector2.right * directionFlipper * speed * Time.deltaTime * 100);
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		midAir = false;

		if (!canJump) {
			//Make it so the player can jump again
			canJump = true;
		}
	}

	void OnCollisionStay2D (Collision2D collision) {
		midAir = false;
	}

	void OnCollisionExit2D (Collision2D collision) {
		midAir = true;
	}
}