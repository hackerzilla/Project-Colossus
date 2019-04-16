using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameObject player;
	private Transform playerTransform;
	private Transform camTransform;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerTransform = player.transform;
		camTransform = this.GetComponent<Transform> ();
	}

	void Update () {

		Vector3 newPos = new Vector3 (playerTransform.position.x, playerTransform.position.y, camTransform.position.z);

		camTransform.position = newPos;

	}
}
