using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCLone : MonoBehaviour {

	public Object ropePrefab;
	public Object simulatedRopePrefab;
	public int numberOfSegmentsNeeded;
	static public float lengthOfSegment;

	private Rigidbody2D rb;
	private Vector2 pos; //look to GrappleHook.cs
	private bool ropeSpawned;
	private Transform playerTransform;


	public float launchForce;


	void Start () {
		lengthOfSegment = 0.1f;
		pos = GrappleHook.mousePos;
		rb = GetComponent<Rigidbody2D> ();
		LaunchHook (pos);
		ropeSpawned = false;
		playerTransform = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
	}


	void LaunchHook (Vector2 pos) {
		rb.AddForce (pos * launchForce, ForceMode2D.Impulse); 
	}

	void OnCollisionEnter2D (Collision2D col) {

		//make the hook object's rigidbody static
		rb.bodyType = RigidbodyType2D.Static;

		//remove the parent attachment of the player
		gameObject.transform.parent = null;



		//Instantiate (ropePrefab, this.transform);

		//^ this one OR v this one (not both)
		if (!ropeSpawned) {
			ropeSpawned = true;	
			Instantiate (simulatedRopePrefab, this.transform);
		}

		float distanceToPlayer = Vector2.Distance (this.transform.position, playerTransform.position);
		Debug.Log ("distance to player:" + distanceToPlayer);


		//gets the number of segments needed, rounded up
		numberOfSegmentsNeeded = Mathf.CeilToInt((distanceToPlayer - lengthOfSegment) / lengthOfSegment);
		Debug.Log ("number of segments needed" + numberOfSegmentsNeeded);

	}
}
