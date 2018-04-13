using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerlineController : MonoBehaviour, IActor {

	private Color startColor;
	private List<string> entities;
	private Dictionary<string, string> actions;
	public List<GameObject> goImPowering;			// list of gameObjects i am sending power to
	public List<GameObject> powerSources;			// list of gameObjects power is send to me
	public List<string> powerOrigins;				// list of unique power origins (button, player..)

	void Start() {
		startColor = GetComponent<SpriteRenderer> ().color;
		goImPowering = new List<GameObject> {};
		powerSources = new List<GameObject> {};
		powerOrigins = new List<string> {};

		entities = new List<string> { "conductive" };
		actions = new Dictionary<string, string> (){{"poweron", "false"},{"poweredByPlayer", "false"},{"color", ""}};
		//connectedConductors = new List<IActor>() ;
	}

	public void Trigger(Dictionary<string, string> _actions, List<string> _entities, GameObject source) {
		if (_actions.ContainsKey("poweron") && _actions ["poweron"] != "false") {
			actions["color"] = _actions ["color"];
			ConductElectricity (true, _actions ["color"], _actions, source);
		}
		if (_actions.ContainsKey("poweroff") && _actions ["poweroff"] != "false" && powerSources.Contains(source)) {
			ConductElectricity (false, "", _actions, source);
		}
	} 

	private void ConductElectricity(bool isAddingPower, string color, Dictionary<string, string> _actions, GameObject source) {
		if (isAddingPower) {

			if (!powerOrigins.Contains (_actions ["poweron"])) { 
				AddPower (source, color, _actions ["poweron"]);
			}

		} else {

			RemovePower (source, _actions ["poweroff"]);	
		}
	}

	private void AddPower(GameObject source, string color, string powerOrigin) {
		
		powerSources.Add (source);
		powerOrigins.Add (powerOrigin);

		// turn self on if offline
		if (powerSources.Count >= 1 && powerSources.Count-1 == 0) {
			actions ["color"] = color;
			Color newCol;
			ColorUtility.TryParseHtmlString (color, out newCol);
			GetComponent<SpriteRenderer> ().color = newCol;
			actions["poweron"] = powerOrigin;

		}

		// power all connected IActor
		Collider2D[] toPowerCols = new Collider2D[128];
		ContactFilter2D cf2d = new ContactFilter2D();
		cf2d.useTriggers = true;
		int colCount = Physics2D.OverlapCollider (GetComponent<BoxCollider2D>(),cf2d, toPowerCols);

		if (colCount > toPowerCols.Length) {
			Debug.Log ("Error: more collider found than fit in array, discarding rest");
		}

		foreach (Collider2D col in toPowerCols) {
			if (col != null) {	// toPowerCols.Length is 128
				if (col.gameObject.GetComponent<IActor> () != null && !powerSources.Contains (col.gameObject)) {
					col.gameObject.GetComponent<IActor> ().Trigger (actions, entities, gameObject);
					if (col.gameObject.name != gameObject.name && !goImPowering.Contains (col.gameObject)) {
						goImPowering.Add (col.gameObject);
					}
				}
			}
		}

	}

	private void RemovePower(GameObject source, string powerOirign) {
		
		powerSources.Remove (source);
		powerOrigins.Remove (powerOirign);

		// remove power from all connected IActor
		for (int i = 0; i < goImPowering.Count; i++) {
			goImPowering [i].GetComponent<IActor> ().Trigger (new Dictionary<string, string> () { { "poweroff", powerOirign } }, entities, gameObject);
		}

		// turn self offline if online
		if (powerSources.Count == 0 && powerSources.Count + 1 == 1) {
			//if (actions ["poweredByPlayer"] == "true") {

			//}
			//actions ["poweredByPlayer"] = "false";
			GetComponent<SpriteRenderer> ().color = startColor;
			actions ["poweron"] = "false";

			goImPowering.Clear ();
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<IActor> () != null && actions.ContainsKey("poweron") && actions["poweron"] != "false" && !goImPowering.Contains(other.gameObject)) {
			goImPowering.Add (other.gameObject);
			other.GetComponent<IActor> ().Trigger (actions, entities, gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<IActor> () != null && actions.ContainsKey("poweron") && actions ["poweron"] != "false" && goImPowering.Contains(other.gameObject)) {
			goImPowering.Remove (other.gameObject);
			// TODO: dont just send the last powerOrigin in List, send the right one..
			other.GetComponent<IActor> ().Trigger (new Dictionary<string, string>() {{"poweroff", powerOrigins [powerOrigins.Count-1]}}, entities, gameObject);
		}
	}
		
}
