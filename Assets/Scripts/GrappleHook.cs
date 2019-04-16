using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour {

	//This script will govern the Grappling Hook mechanic
	//The grappling hook will be fired from the player towards the location of the mouse curser (when the player clicks the left mousebutton)
	//The first object in the grappling hook's trajectory will be the hook's anchor point
	//Then the player can swing from the hook anchored at that location
	//When the player clicks the left mouse button again, the hook will let go (and reel back in)


	Camera myCam;
	Transform playerTransform;
	PlayerController player;
	private bool hookExists;
	private GameObject hook;
	//private float timeSinceLastChange = 0f;

	public Object hookPrefab;
	public float rateOfChangeRopeLength;

	static public Vector2 mousePos;


	void Start () {
		hookExists = false;
		myCam = Camera.main;
		player = FindObjectOfType<PlayerController> ();
		playerTransform = player.gameObject.transform;
	}

	void Update () {
		//Get the mouse's position into a vector 2 (as if the center of the camera's view is the origin)

		Vector3 mousePosition = myCam.ScreenToWorldPoint(Input.mousePosition);
		//Debug.Log (Input.mousePosition);
		//Debug.Log (myCam.ScreenToWorldPoint(Input.mousePosition));

		mousePos = new Vector2 (mousePosition.x - playerTransform.position.x, mousePosition.y - playerTransform.position.y);
		//Debug.Log (mousePos);

		if (Input.GetKeyDown (KeyCode.Mouse0) && !hookExists) {
			
			FireHook (Input.mousePosition);

		} else if (Input.GetKeyDown (KeyCode.Mouse0) && hookExists) {
			//Destroy the existing hook

			hook = GameObject.FindObjectOfType<TestRope> ().gameObject;
			Destroy (hook);

			hookExists = false;
		}


		//handles W and S input
		//decriment or incriment the length of the rope by 1, depending on the input
		//the rate of incrementation or decrementation should be limited based on time eg. 1 tick per second

//		if (hookExists) {
//			//IncreaseOrDecreaseRopeLength (true); //for testing
//			if (Input.GetKey (KeyCode.W)) {
//				//Reduce the length of the Rope (Distance Joint 2D)
//				IncreaseOrDecreaseRopeLength(false);
//			} else if (Input.GetKey (KeyCode.S)) {
//				//Increase the length of the Rope
//				IncreaseOrDecreaseRopeLength(true);
//			}
//		}

	}

	void FireHook(Vector2 mousePos) {
		//Instatiate a Hook prefab and give it force in the dircetion of mousePos

		//Create a new hook
		Instantiate (hookPrefab, transform.position, Quaternion.identity);

		hookExists = true;
		//Give the hook force in the direct of the mousePos

	}


//	void IncreaseOrDecreaseRopeLength(bool increment) {
//		int change = 0;
//		//if increment is true then increment if false then decrement
//		if (increment) {
//			//increase
//			change = 1;
//		} else if (!increment) {
//			//decrease
//			change = -1;
//		}
//
//		//TODO replace 1 with rateOfChangeRopeLength
//		if (Time.time - timeSinceLastChange > rateOfChangeRopeLength) {
//			//increment or decrement the length of the rope here
//
//			GameObject simulatedRope = GameObject.Find ("Simulated Rope(Clone)");
//			
//			simulatedRope.GetComponent<SimulatedRope>().ChangeRopeLength (change);
//
//			Debug.Log ("Change the length of the rope by " + change);
//			timeSinceLastChange = Time.time;
//		}
//	}
}
