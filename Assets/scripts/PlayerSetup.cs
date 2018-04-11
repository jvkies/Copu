using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	public Behaviour[] componentsToDisable;

	void Start() {
		
		if (!isLocalPlayer) {
			foreach (Behaviour component in componentsToDisable) {
				component.enabled = false;
			}
		} else {
			if (GameObject.FindGameObjectWithTag ("MainCamera") != null) {
				GameObject.FindGameObjectWithTag ("MainCamera").SetActive (false);
			}
			Debug.Log ("this is player 1?");
		}
	}
}
