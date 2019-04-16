using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHook : MonoBehaviour {


	public GameObject hookPrefab;


	void Start () {
		
	}
	
	void Update () {

		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			Instantiate (hookPrefab, transform.position, Quaternion.identity);
		}
	}
}
