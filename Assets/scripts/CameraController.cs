using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	public GameObject player;

	void Start () {
		//player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void Update () {
		if (player != null) {
			transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
		}
	}

	// this is for single player
	public void SwitchCameraFocus(string newPlayerTag) {
		if (GameManager.instance.isPlayerAbleToInteract) {
			player = GameObject.FindGameObjectWithTag (newPlayerTag);
		}
	}
}
