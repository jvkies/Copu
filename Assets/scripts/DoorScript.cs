using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour, IPowerable {

	private Color startColor;
	private Color highlightColor;
	private List<string> powerSourceOrigins;

	public int powerSourcesToOpen = 1;		// amount of connected power sources to open, if its 1 its a normal door
	public bool isReverseDoor = false;
	public Text amountNeeedText;
	public GameObject door;

	void Start() {
		startColor = door.GetComponent<SpriteRenderer> ().color;
		powerSourceOrigins = new List<string> ();
		SetPowerSourcedNeededText ();

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

		highlightColor = Util.TryParseHtmlString (color);

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
		
	public void Interact(bool isActivating) {

		SetPowerSourcedNeededText ();

		if (powerSourceOrigins.Count >= powerSourcesToOpen) {

			if (isReverseDoor) {
				OpenDoor (false);
			} else {
				OpenDoor (true);
			}

			if (amountNeeedText != null) {
				amountNeeedText.enabled = false;
			}
		} else {
			
			if (isReverseDoor) {
				OpenDoor (true);
			} else {
				OpenDoor (false);
			}

			if (amountNeeedText != null) {
				amountNeeedText.enabled = true;
			}
		}
	}

	private void OpenDoor(bool open) {
		if (open) {
			door.transform.localPosition = new Vector2(door.transform.localPosition.x, 1);
			if (isReverseDoor) {
				door.GetComponent<SpriteRenderer> ().color = startColor;
			} else {
				door.GetComponent<SpriteRenderer> ().color = highlightColor;
			}

		} else {
			door.transform.localPosition = new Vector2(door.transform.localPosition.x, -0.01f);

			if (isReverseDoor) {
				door.GetComponent<SpriteRenderer> ().color = highlightColor;
			} else {
				door.GetComponent<SpriteRenderer> ().color = startColor;
			}
		}
	}

	void SetPowerSourcedNeededText() {
		if (powerSourcesToOpen != 1) {
			amountNeeedText.text = (powerSourcesToOpen-powerSourceOrigins.Count).ToString();
		}
	}
}
