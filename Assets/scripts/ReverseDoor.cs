using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReverseDoor : MonoBehaviour, IPowerable {

	//	private int powerSourcesCount;
	private List<string> powerSourceOrigins;

	public GameObject door;

	void Awake() {
		powerSourceOrigins = new List<string> ();
	}

	public void AddPower (GameObject source, string color, List<string> _powerOrigins) {
		foreach (string powerOrigin in _powerOrigins.ToArray()) {
			AddPower (source, color, powerOrigin);
		}
	}

	public void AddPower (GameObject source, string color, string powerOrigin) {
		if (!powerSourceOrigins.Contains(powerOrigin) && source.tag != "Player") {
			powerSourceOrigins.Add (powerOrigin);
		}

		Interact(true);

	}

	public void RemovePower (GameObject source, List<string> _powerOrigins) {
		foreach (string powerOrigin in _powerOrigins.ToArray()) {
			RemovePower (source, powerOrigin);
		}

	}

	public void RemovePower (GameObject source, string powerOrigin) {
		if (powerSourceOrigins.Contains(powerOrigin)) {
			powerSourceOrigins.Remove (powerOrigin);
		}

		Interact(false);

	}

	private void OpenDoor(bool open) {
		if (open) {
			door.transform.localPosition = new Vector2(door.transform.localPosition.x, 1);
		} else {
			door.transform.localPosition = new Vector2(door.transform.localPosition.x, -0.01f);
		}
	}

	public void Interact(bool isActivating) {

		if (powerSourceOrigins.Count >= 1) {
			OpenDoor (false);
			//door.SetActive (true);
		} else {
			OpenDoor (true);
			//door.SetActive (false);
		}
	}

}
