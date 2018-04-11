﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour, ITrigger {

	private float pressureTreshold = 1f;	// how much pressure needed to push the button
	private Color startColor;
	private GameObject interactableElement;

	public string elementToInteractWithTag;
	public float weightToReact;
	public float pressureAmount;	// current amount of pressure on the button
	public Color32 highlightColor = new Color32(255,30,30,255);
	public GameObject[] indicators;	// idicators to be highlighted

	// Use this for initialization
	void Start () {
		startColor = GetComponent<SpriteRenderer> ().color;
		try {
			interactableElement = GameObject.FindGameObjectWithTag (elementToInteractWithTag);
		} catch {
			Debug.Log ("PressureButton couldn't find interactable: " + elementToInteractWithTag);
		}
	}
	
	public void Trigger(List<string> entities, float mass) {
		if (entities.Contains ("pressure")) {
			ChangePressure (mass);
		}
	}

	public void StopTrigger(List<string> entities, float mass) {
		if (entities.Contains ("pressure")) {
			ChangePressure (-mass);
		}
	}

	public void ChangePressure(float amount) {
		pressureAmount += amount;

		if (pressureAmount >= pressureTreshold && pressureAmount-amount < pressureTreshold) {
			GetComponent<SpriteRenderer> ().color = highlightColor;
			interactableElement.GetComponent<IActor> ().Interact (true);
			HighlightIndicators (true);
		} else if (pressureAmount < pressureTreshold && pressureAmount-amount > pressureTreshold) {
			GetComponent<SpriteRenderer> ().color = startColor;
			interactableElement.GetComponent<IActor> ().Interact (false);
			HighlightIndicators (false);
		}
	}

	public void HighlightIndicators(bool isHighlighting) {
		foreach (GameObject go in indicators) {
			if (isHighlighting) {
				go.GetComponent<SpriteRenderer> ().color = highlightColor;
			} else {
				go.GetComponent<SpriteRenderer> ().color = startColor;
			}
		}
	}
}