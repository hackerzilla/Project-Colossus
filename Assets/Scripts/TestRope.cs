using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRope : MonoBehaviour {


	public Object nodePrefab;

	private Rigidbody2D rb;

	public float launchForce = 1f;

	private Transform player;

	private Vector2 previousNodePosition;

	public float intervalDistance = .5f;

	public List<GameObject> Nodes;

	public GameObject previousNode;

	public float ascensionSpeed = 40f;

	public bool touchdown = false;

	public bool newNodeConnected = true;

	[Range(0,1000000)]
	public float springJointFrequency = 5f;

	public float springJointDistance = 0.005f;

	public float springJointDeletionDistanceThreshold = .01f;

	[Range(0,1)]
	public float springJointDampingRatio = 1f;

	[SerializeField()]
	public int node1Index = 1;
	public int node2Index = 2;
		


	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("Player").transform;

		previousNodePosition = player.position;

		previousNode = player.gameObject;

		rb = GetComponent<Rigidbody2D> ();

		//Weeeeeeeeeee
		LaunchHook ();

	}


	void Update () {

		float distanceToLastNode = Vector2.Distance(transform.position, previousNodePosition);


		//this if statement block must come before the next one
		if (!newNodeConnected) {

			GameObject newNode = GameObject.FindGameObjectWithTag ("New Node");

			float distance = Vector2.Distance (player.position, newNode.transform.position);

			if (distance >= intervalDistance) {

				newNode.GetComponent<HingeJoint2D> ().connectedBody = player.gameObject.GetComponent<Rigidbody2D> ();

				newNode.tag = "Untagged";

				newNodeConnected = true;

			}

		}

		if (!touchdown) {
			
			if (distanceToLastNode >= intervalDistance ) {

				CreateNode ();

			}
		} else if (Input.GetKey(KeyCode.S)) {

			Rappel ();
			
		} else {

			GetComponent<HingeJoint2D> ().enabled = true;

			GetComponent<HingeJoint2D> ().connectedBody = previousNode.GetComponent<Rigidbody2D>();

		}

		if (Input.GetKey (KeyCode.W)) {

			Ascend ();

		} else {

		}

		RenderLine ();

	}

	/// --------------------------------------------------------UPDATE LOOP ^---------------------------------------------


	void CreateNode () {
		
		GameObject node = (GameObject)Instantiate (nodePrefab, transform);

		node.GetComponent<HingeJoint2D> ().connectedBody = previousNode.GetComponent<Rigidbody2D>();

		previousNodePosition = node.transform.position;

		previousNode = node;

		Nodes.Add (node);

	}

	void LaunchHook () {
		
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		Vector2 difference = mousePos - new Vector2 (player.position.x, player.position.y);

		Vector2 forceVector = difference.normalized;


//		Debug.Log ("mouse position: " + mousePos);
//		Debug.Log ("difference between mouse position and player position: " + difference);
//		Debug.Log ("force vector: " + forceVector);


		rb.AddForce (forceVector * launchForce, ForceMode2D.Impulse);

	}


	void Rappel () {

		for (int i=0; i < Nodes.Count; i++) {

			GameObject node = Nodes [i];

			HingeJoint2D joint = node.GetComponent<HingeJoint2D> ();

			if (joint.connectedBody && joint.connectedBody.gameObject.tag == "Player") {
				
				float distance = Vector2.Distance (player.transform.position, node.transform.position);

				if (distance >= intervalDistance) {
					
					GameObject newNode = (GameObject)Instantiate (nodePrefab, player.position, Quaternion.identity, transform);

					node.GetComponent<HingeJoint2D> ().connectedBody = newNode.GetComponent<Rigidbody2D> ();

					newNode.tag = "New Node";
			
					Nodes.Insert (0, newNode);

					newNodeConnected = false;

				}
			}
		}
	}


	void Ascend () {


		GameObject node = Nodes[0];

		GameObject newNode = Nodes [1];

		Vector3 difference = node.transform.position - player.transform.position;

		Debug.Log ("difference: " + difference);

		difference.Normalize ();

		difference = difference * Time.deltaTime * ascensionSpeed;

		player.transform.position += difference;

		if (Vector2.Distance(player.transform.position, node.transform.position) <= springJointDeletionDistanceThreshold ) {

			Destroy(node);

			Nodes.RemoveAt(0);

			newNode.GetComponent<HingeJoint2D> ().connectedBody = player.GetComponent<Rigidbody2D> ();

		}

	}



	void RenderLine () {

		//this for loop should run the list backwards because 

		LineRenderer lineRenderer = GetComponent<LineRenderer> ();

		lineRenderer.positionCount = Nodes.Count;

		for (int i = 0; i < Nodes.Count; i++) {

			lineRenderer.SetPosition (i, Nodes [i].transform.position);

		}

	}



	void OnCollisionEnter2D (Collision2D col) {
		
		rb.bodyType = RigidbodyType2D.Kinematic;

		rb.freezeRotation = true;

		rb.velocity = Vector2.zero;

		touchdown = true;

	}
}
