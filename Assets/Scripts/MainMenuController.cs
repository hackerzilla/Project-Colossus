using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	void Update () {

		if (Input.anyKey) {
			//Start game
			SceneManager.LoadScene(1);
		}
	}
}
