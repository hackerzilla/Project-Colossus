using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedRope : MonoBehaviour {

	//This script is for the simulated rope object
	//When it is instantiated, it should attach the first segment's hinge joint to the hook object ...
	//And attach the last segment's hinge joint to the player

	public Object segmentPrefab;

	private GameObject hook, player;
	private DistanceJoint2D attachmentToHook;
	private HingeJoint2D attachmentToPlayer;
	private Rigidbody2D hookRigidBody2D;
	private Rigidbody2D playerRigidBody2D;
	private GameObject firstSegment;



	void Start () {
		//finds the first segment of the simulated rope prefab
		firstSegment = GameObject.FindGameObjectWithTag ("FirstSegment");
		hook = GetComponentInParent<HookCLone> ().gameObject;
		player = GameObject.FindGameObjectWithTag ("Player");

		this.transform.position += new Vector3 (0f, HookCLone.lengthOfSegment * -1f, 0f);
		//create the rope out of a string of connected segments
		CreateRope ();
		//rotate the rope towards the player
		RotateTowardsPlayer (this.gameObject);



		//finds the last segment of the rope
	
		GameObject lastSegment = GameObject.FindGameObjectWithTag ("LastSegment");


		//gets the hook's rigidbody component
		hookRigidBody2D = hook.GetComponent<Rigidbody2D>();

		//gets the first segment of the rope's distancejoint2d component
		attachmentToHook = firstSegment.GetComponent<DistanceJoint2D> ();

		//gets the last segment in the rope's hingejoint2d component
		attachmentToPlayer = lastSegment.GetComponent<HingeJoint2D> ();

		//gets the player gameobject's rigidbody2d component
		playerRigidBody2D = player.GetComponent<Rigidbody2D>();



		//assigns the firt segment of the rope's anchor as the hook's rigidbody
		attachmentToHook.connectedBody = hookRigidBody2D;

		//assigns the player's rigidbody as the connection to the last segment of the rope
		attachmentToPlayer.connectedBody = playerRigidBody2D;
	}
	
	void Update () {
//		GameObject testChild = transform.GetChild (transform.childCount -1).gameObject;
//		testChild.GetComponentInChildren<SpriteRenderer> ().color = Color.red;
	}


	void CreateRope() {
		//create a rope of a certain length of segments using the Public Int numberOfSegmentsNeeded variable from the HooCLone script


		HingeJoint2D previousSegmentHinge = firstSegment.GetComponent<HingeJoint2D> ();

		for (int i = 0; i < hook.GetComponent<HookCLone>().numberOfSegmentsNeeded ;i++) {
			//create the segment and lower its transform depending on its position in the rope

			//a calculation of how far down on the Y axis the segment needs to be, given its position in the for loop (i)
			float transformationY = firstSegment.transform.localPosition.y - ((i + 1) * HookCLone.lengthOfSegment); //i + 1 is because the for loop starts i off as 0

			GameObject segment = (GameObject)Instantiate (segmentPrefab, this.transform);

			//moves the segment along the Y axis in the amount of transformationY
			segment.GetComponent<Transform> ().localPosition += new Vector3 (0, transformationY, 0);

			previousSegmentHinge.connectedBody = segment.GetComponent<Rigidbody2D> ();
			
			if (i == hook.GetComponent<HookCLone> ().numberOfSegmentsNeeded - 1) {


				segment.gameObject.tag = "LastSegment";
				//resets the variable to default value
				previousSegmentHinge = firstSegment.GetComponent<HingeJoint2D> ();
				Destroy(transform.GetChild (i).GetComponent<HingeJoint2D>());
				transform.GetChild (i).gameObject.AddComponent(typeof(SpringJoint2D));


			} else if (i == hook.GetComponent<HookCLone>().numberOfSegmentsNeeded - 2) {
				
			} else {
				//set the previousSegmentHinge equal to this segment's hinge for the next iteration of the loop to use
				previousSegmentHinge = segment.GetComponent<HingeJoint2D>();
			}
			
		}
	}

	void RotateTowardsPlayer (GameObject objectToRotate) {
		Vector3 differenceToPlayer = hook.transform.position - player.transform.position;

		differenceToPlayer.Normalize ();

		float rotation = Mathf.Atan2 (differenceToPlayer.y, differenceToPlayer.x) * Mathf.Rad2Deg;
		objectToRotate.transform.rotation = Quaternion.Euler (0f,0f, rotation - 90);
	}

	//public so it can be called from GrappleHook.cs
	public void ChangeRopeLength(int change) {
		//change is either 1 or -1 (increment and decrement respectively)
		GameObject oldLastSegment = GameObject.FindGameObjectWithTag("LastSegment");

		GameObject oldSecondToLastSegment = this.transform.GetChild (this.transform.childCount - 2).gameObject;

		//TODO fix this broken shit V

		if (change == 1) {
			//spawning in a segment and adding it onto the rope 

			GameObject newLastSegment = (GameObject)Instantiate (segmentPrefab, this.transform);
			newLastSegment.transform.localPosition = oldLastSegment.transform.localPosition;
			newLastSegment.transform.position = player.transform.position;


			//find the normalized vector between oldLastSegment to the player
			//rotate the newLastSegment by that normalized vector's euler angle

			Vector2 playerToOldLastSegmentDifference = player.transform.position - oldLastSegment.transform.position;
			playerToOldLastSegmentDifference.Normalize ();
			Quaternion newLastSegmentRotation = Quaternion.Euler (playerToOldLastSegmentDifference);
			newLastSegment.transform.localRotation = newLastSegmentRotation;



			newLastSegment.transform.localPosition += new Vector3 (0f, -1f * HookCLone.lengthOfSegment, 0f);

			oldLastSegment.tag = "Untagged";
			newLastSegment.tag = "LastSegment";
			oldLastSegment.GetComponent<HingeJoint2D> ().connectedBody = newLastSegment.GetComponent<Rigidbody2D> ();
			newLastSegment.GetComponent<HingeJoint2D> ().connectedBody = playerRigidBody2D;


			player.transform.position = newLastSegment.transform.position;
		} else if (change == -1) {
			oldSecondToLastSegment.tag = "LastSegment";
			oldSecondToLastSegment.GetComponent<HingeJoint2D> ().connectedBody = playerRigidBody2D;
			player.transform.position = oldSecondToLastSegment.transform.position;
			Destroy (oldLastSegment);
		}
	}
}
