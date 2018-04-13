using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour, IActor {

	private int powerSourcesCount;

	public int powerSourcesToOpen = 1;		// amount of connected power sources to open, if its 1 its a normal door
	public Text amountNeeedText;
	public GameObject door;

	void Start() {
		SetPowerSourcedNeededText ();
	}

	public void Trigger(Dictionary<string, string> actions, List<string> entities, GameObject source) {
		if (actions.ContainsKey("poweron") && actions ["poweron"] != "false") {
			Interact(true);
		}
		if (actions.ContainsKey("poweroff") && actions ["poweroff"] != "false") {
			Interact(false);
		}

	}
		
	public void Interact(bool isActivating) {
		Debug.Log ("interacting");
		if (isActivating) { powerSourcesCount++; } 
		else { powerSourcesCount--; }

		SetPowerSourcedNeededText ();

		if (powerSourcesCount >= powerSourcesToOpen) {
			door.SetActive (false);
			if (amountNeeedText != null) {
				amountNeeedText.enabled = false;
			}
		} else {
			door.SetActive (true);
			if (amountNeeedText != null) {
				amountNeeedText.enabled = true;
			}
		}
	}

	void SetPowerSourcedNeededText() {
		if (powerSourcesToOpen != 1) {
			amountNeeedText.text = (powerSourcesToOpen-powerSourcesCount).ToString();
		}
	}
}
