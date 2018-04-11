using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, IActor {

	private int interactCounter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Interact(bool isActivating) {

		if (isActivating) { interactCounter++; } 
		else { interactCounter--; }

		if (interactCounter >= 1) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive (true);
		}
	}
}
