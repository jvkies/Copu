using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour, IActor {

	private float pressureTreshold = 1f;	// how much pressure needed to push the button
	private Color startColor;
	private List<string> entities;
	private Dictionary<string,string> actions;
	public List<GameObject> goImPowering;

	public float weightToReact;
	public float currentPressure;	// current amount of pressure on the button
	public Color32 highlightColor = new Color32(255,30,30,255);

	//public enum Orientation{ Left, Right, Top, Bottom };
	//public Orientation orientation;

	// Use this for initialization
	void Start () {

		entities = new List<string> {};
		actions = new Dictionary<string, string> (){{"poweron", "false"},{"poweroff", "false"}, {"color", "#"+ColorUtility.ToHtmlStringRGBA(highlightColor)}};
		goImPowering = new List<GameObject> {};

		startColor = GetComponent<SpriteRenderer> ().color;

	}
	
	public void Trigger(Dictionary<string, string> _actions, List<string> _entities, GameObject source) {
		if (_entities.Contains ("pressure") && _actions.ContainsKey("mass")) {
			ChangePressure (float.Parse(_actions["mass"]));
		}
	}
		
	public void ChangePressure(float amount) {
		
		currentPressure += amount;

		if (currentPressure >= pressureTreshold && currentPressure-amount < pressureTreshold) {
			GetComponent<SpriteRenderer> ().color = highlightColor;
			actions ["poweron"] = gameObject.name;
			actions ["poweroff"] = "false";

			Collider2D[] toPowerCols = Physics2D.OverlapCircleAll (gameObject.transform.position, 0.35f);
			foreach (Collider2D col in toPowerCols) {
				if (col.gameObject.GetComponent<IActor> () != null) {
					col.gameObject.GetComponent<IActor> ().Trigger (actions, entities, gameObject);
					if (col.gameObject.name != gameObject.name) {
						goImPowering.Add (col.gameObject);
					}
				}
			}

		} else if (currentPressure < pressureTreshold && currentPressure-amount > pressureTreshold) {
			GetComponent<SpriteRenderer> ().color = startColor;
			actions ["poweron"] = "false";
			actions ["poweroff"] = gameObject.name;

			for (int i = 0; i <= goImPowering.Count; i++) {
				goImPowering[0].GetComponent<IActor>().Trigger(actions, entities, gameObject);
				goImPowering.RemoveAt(0);
			}

		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<IActor> () != null && actions.ContainsKey("poweron") && actions["poweron"] == gameObject.name && !goImPowering.Contains(other.gameObject)) {
			goImPowering.Add (other.gameObject);
			other.GetComponent<IActor> ().Trigger (actions, entities, gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<IActor> () != null && actions.ContainsKey("poweron") && actions ["poweron"] == gameObject.name) {
			goImPowering.Remove (other.gameObject);
			other.GetComponent<IActor> ().Trigger (new Dictionary<string, string>() {{"poweroff", gameObject.name}}, entities, gameObject);

		}
	}


}
