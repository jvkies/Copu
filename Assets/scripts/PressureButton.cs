using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour, IPressureable {

	private bool isActive;					// wether the button is pressed and sending power
	private float pressureTreshold = 1f;	// how much pressure needed to push the button
	private Color startColor;
	public List<GameObject> goImPowering;

	public float weightToReact;
	public float currentPressure;	// current amount of pressure on the button
	public Sprite buttonActive;
	private Sprite buttonInactive;
	public Color32 highlightColor = new Color32(255,30,30,255);

	//public enum Orientation{ Left, Right, Top, Bottom };
	//public Orientation orientation;

	// Use this for initialization
	void Start () {
		goImPowering = new List<GameObject> {};

		startColor = GetComponent<SpriteRenderer> ().color;
		buttonInactive = GetComponent<SpriteRenderer> ().sprite;
	}

	public void AddPressure(float amount) {
		
		currentPressure += amount;

		// pressure threshold reached
		if (currentPressure >= pressureTreshold && currentPressure - amount < pressureTreshold) {
			ActivateButton ();
		}
	}

	public void RemovePressure(float amount) {

		currentPressure -= amount;

		if (currentPressure < pressureTreshold && currentPressure + amount > pressureTreshold) {
			DeactivateButton ();
		}
	}

	private void ActivateButton() {
		isActive = true;

		GetComponent<SpriteRenderer> ().color = highlightColor;
		GetComponent<SpriteRenderer> ().sprite = buttonActive;

		// send power to all colliding objects
		Collider2D[] toPowerCols = Physics2D.OverlapCircleAll (gameObject.transform.position, 0.35f);
		foreach (Collider2D col in toPowerCols) {
			if (col.gameObject.GetComponent<IPowerable> () != null) {
				col.gameObject.GetComponent<IPowerable> ().AddPower (gameObject, "#" + ColorUtility.ToHtmlStringRGBA (highlightColor), gameObject.name);
				if (col.gameObject.name != gameObject.name) {
					goImPowering.Add (col.gameObject);
				}
			}
		}
	}

	private void DeactivateButton() {
		isActive = false;

		GetComponent<SpriteRenderer> ().color = startColor;
		GetComponent<SpriteRenderer> ().sprite = buttonInactive;

		if (goImPowering.Count != 0) {
			for (int i = 0; i <= goImPowering.Count; i++) {
				goImPowering [0].GetComponent<IPowerable> ().RemovePower (gameObject, gameObject.name);
				goImPowering.RemoveAt (0);
			}
		}
	}
		
	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<IPowerable> () != null && isActive && !goImPowering.Contains(other.gameObject)) {
			goImPowering.Add (other.gameObject);
			other.GetComponent<IPowerable> ().AddPower (gameObject, "#"+ColorUtility.ToHtmlStringRGBA(highlightColor), gameObject.name);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<IPowerable> () != null && isActive) {
			goImPowering.Remove (other.gameObject);
			other.GetComponent<IPowerable> ().RemovePower (gameObject, gameObject.name);

		}
	}


}
