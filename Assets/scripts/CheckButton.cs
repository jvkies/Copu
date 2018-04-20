using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckButton : MonoBehaviour {

	private bool isActive;					// wether the button is pressed and sending power
	private Color startColor;
	public List<GameObject> goImPowering;

	public Sprite buttonActive;
	private Sprite buttonInactive;
	public Color32 highlightColor = new Color32(255,30,30,255);

	//public enum Orientation{ Left, Right, Top, Bottom };
	//public Orientation orientation;

	// Use this for initialization
	void Awake () {
		goImPowering = new List<GameObject> {};

		startColor = GetComponent<SpriteRenderer> ().color;
		buttonInactive = GetComponent<SpriteRenderer> ().sprite;
	}
		
	public void ActivateButton() {
		isActive = true;

		GetComponent<SpriteRenderer> ().color = highlightColor;
		GetComponent<SpriteRenderer> ().sprite = buttonActive;

		Collider2D[] toPowerCols = Physics2D.OverlapBoxAll (gameObject.transform.position, GetComponent<Collider2D> ().bounds.size, 0);
		foreach (Collider2D col in toPowerCols) {
			if (col.gameObject.GetComponent<IPowerable> () != null) {
				if (col.gameObject.name != gameObject.name) {
					goImPowering.Add (col.gameObject);
				}
				col.gameObject.GetComponent<IPowerable> ().AddPower (gameObject, "#" + ColorUtility.ToHtmlStringRGBA (highlightColor), gameObject.name);
			}
		}
	}

	public void DeactivateButton() {
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
		if (isActive) {
			DeactivateButton ();
		} else {
			ActivateButton ();
		}
	//	if (other.GetComponent<IPowerable> () != null && isActive && !goImPowering.Contains(other.gameObject)) {
	//		goImPowering.Add (other.gameObject);
	//		other.GetComponent<IPowerable> ().AddPower (gameObject, "#"+ColorUtility.ToHtmlStringRGBA(highlightColor), gameObject.name);
	//	}
	}

	void OnTriggerExit2D(Collider2D other) {
	//	DeactivateButton ();
		if (other.GetComponent<IPowerable> () != null && isActive) {
			goImPowering.Remove (other.gameObject);
			other.GetComponent<IPowerable> ().RemovePower (gameObject, gameObject.name);
		}
	}


}
