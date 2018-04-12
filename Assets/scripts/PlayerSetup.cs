using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	public Behaviour[] componentsToDisable;

	void Start() {

		if (GameManager.instance.gameMode == "lan") {
			if (!isLocalPlayer) {
				foreach (Behaviour component in componentsToDisable) {
					component.enabled = false;
				}
			} else {
				if (GameObject.FindGameObjectWithTag ("MainCamera") != null) {
					GameObject.FindGameObjectWithTag ("MainCamera").SetActive (false);
				}
			}
		}
	}
}
