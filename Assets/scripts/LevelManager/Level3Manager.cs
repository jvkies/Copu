using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : MonoBehaviour {

	public List<GameObject> checkButtonToActivate;

	// Use this for initialization
	void Start () {
	//	checkButtonToActivate = new List<GameObject> ();

		foreach (GameObject button in checkButtonToActivate) {
			Debug.Log ("activating: " + button.name);
			button.GetComponent<CheckButton> ().ActivateButton ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
