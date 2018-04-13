using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour, IActor {

	private float pressureTreshold = 1f;	// how much pressure needed to push the button
	private Color startColor;
	private List<GameObject> interactableElements;
	private List<string> entities;
	private Dictionary<string,string> actions;
	public List<GameObject> goImPowering;

	public string[] elementsToInteractWithTag;
	public float weightToReact;
	public float pressureAmount;	// current amount of pressure on the button
	public Color32 highlightColor = new Color32(255,30,30,255);
	public GameObject[] indicators;	// idicators to be highlighted

	// Use this for initialization
	void Start () {
		entities = new List<string> {};
		actions = new Dictionary<string, string> (){{"poweron", "false"},{"poweroff", "false"}, {"color", "#"+ColorUtility.ToHtmlStringRGBA(highlightColor)}};
		goImPowering = new List<GameObject> {};

		interactableElements = new List<GameObject>();
		startColor = GetComponent<SpriteRenderer> ().color;

		try {
			foreach (string tag in elementsToInteractWithTag) {
				foreach (GameObject interactableElement in GameObject.FindGameObjectsWithTag (tag)) {
					interactableElements.Add(interactableElement);
				}
			}
		} catch {
			Debug.Log ("PressureButton couldn't find interactable Tag");
		}
	}
	
	public void Trigger(Dictionary<string, string> _actions, List<string> _entities, GameObject source) {
		if (_entities.Contains ("pressure") && _actions.ContainsKey("mass")) {
			ChangePressure (float.Parse(_actions["mass"]));
		}
	}
		
	public void ChangePressure(float amount) {
		pressureAmount += amount;

		if (pressureAmount >= pressureTreshold && pressureAmount-amount < pressureTreshold) {
			GetComponent<SpriteRenderer> ().color = highlightColor;
			actions ["poweron"] = gameObject.name;
			actions ["poweroff"] = "false";

			Collider2D[] toPowerCols = Physics2D.OverlapCircleAll (gameObject.transform.position, 0.35f);
			foreach (Collider2D col in toPowerCols) {
				//Debug.Log(col.gameObject.name);
				if (col.gameObject.GetComponent<IActor> () != null) {
					col.gameObject.GetComponent<IActor> ().Trigger (actions, entities, gameObject);
					if (col.gameObject.name != gameObject.name) {
						goImPowering.Add (col.gameObject);
					}
				}
			}

			//TriggerPowerline ();

			//foreach (GameObject interactableElement in interactableElements) {
			//	interactableElement.GetComponent<IActor> ().Trigger (actions, entities);
			//}
		} else if (pressureAmount < pressureTreshold && pressureAmount-amount > pressureTreshold) {
			GetComponent<SpriteRenderer> ().color = startColor;
			actions ["poweron"] = "false";
			actions ["poweroff"] = gameObject.name;

			for (int i = 0; i <= goImPowering.Count; i++) {
				goImPowering[0].GetComponent<IActor>().Trigger(actions, entities, gameObject);
				goImPowering.RemoveAt(0);
			}

			//foreach (GameObject go in goImPowering) {
			//	go.GetComponent<IActor>().Trigger(actions, entities);
			//	goImPowering.Remove (go);
			//}

			//Collider2D[] toPowerCols = Physics2D.OverlapCircleAll (gameObject.transform.position, 0.35f);
			//foreach (Collider2D col in toPowerCols) {
				//Debug.Log(col.name);
			//	col.gameObject.GetComponent<IActor>().Trigger(actions, entities);
			//}

			//TriggerPowerline ();

			//foreach (GameObject interactableElement in interactableElements) {
			//	interactableElement.GetComponent<IActor> ().Trigger (actions, entities);
			//}
		}
	}
	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		//Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
		Gizmos.DrawWireSphere (transform.position , 0.35f);
	}


	public void TriggerPowerline() {
		foreach (GameObject go in indicators) {
			// TODO: put the color change in the PowerlineController class

		//	if (isTriggering) {
		//		go.GetComponent<SpriteRenderer> ().color = highlightColor;
		//	} else {
		//		go.GetComponent<SpriteRenderer> ().color = startColor;
		//	}

			go.GetComponent<PowerlineController> ().Trigger (actions, entities, gameObject);
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
