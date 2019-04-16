using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnNet : MonoBehaviour {

	PlayerController player;

	void Start () {
		player = FindObjectOfType<PlayerController> ();
	}
		
	void OnTriggerEnter2D (Collider2D col) {
		if (col.name == "Player") {
			player.Respawn ();
		}
	}
}
