using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {

	private DistanceJoint2D joint;
	private Rigidbody2D playerRigidbody2D;
	//private Transform playerTransform;

	void Start () {
		joint = GetComponent<DistanceJoint2D> ();
		playerRigidbody2D = GetComponentInParent<PlayerController> ().gameObject.GetComponent<Rigidbody2D>();
		//playerTransform = GetComponentInParent<PlayerController> ().gameObject.GetComponent<Transform> ();

		Vector2 anchorTransform = GetComponentInParent<HookCLone> ().gameObject.transform.position;

		//joint.anchor = anchorTransform; ... this line of code breaks it 

		joint.connectedBody = playerRigidbody2D;
	}
	
	void Update () {
		
	}
}
