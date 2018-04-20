using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour, IPowerable {

	private Color startColor;
	//public List<string> entities = new List<string> { "conductive" };
	//public Dictionary<string, string> actions = new Dictionary<string, string> (){{"poweron", "false"},{"color", ""}};

	public List<GameObject> goImPowering = new List<GameObject> {};			// list of gameObjects i am sending power to
	public List<GameObject> powerSources = new List<GameObject> {};			// list of gameObjects power is send to me
	public List<string> powerOrigins;				// list of unique power origins (button, player..)

	void Awake() {
		powerOrigins = new List<string> {};
		startColor = GetComponent<SpriteRenderer> ().color;
	}
		
	public void AddPower(GameObject source, string color, List<string> _powerOrigins) {
		foreach (string powerOrigin in _powerOrigins) {
			AddPower (source, color, powerOrigin);
		}
	}

	public void AddPower(GameObject source, string color, string powerOrigin) {

		if (powerOrigins.Contains (powerOrigin)) { 
			return;
		}

		powerSources.Add (source);
		powerOrigins.Add (powerOrigin);

		// turn self on if offline
		if (powerSources.Count >= 1 && powerSources.Count-1 == 0) {
			GetComponent<SpriteRenderer> ().color = Util.TryParseHtmlString(color);
		}

		// power all connected IActor
		Collider2D[] toPowerCols = new Collider2D[128];
		ContactFilter2D cf2d = new ContactFilter2D();
		cf2d.useTriggers = true;
		int colCount = Physics2D.OverlapCollider (GetComponent<Collider2D>(),cf2d, toPowerCols);

		if (colCount > toPowerCols.Length) {
			Debug.Log ("Error: more collider found than fit in array, discarding rest");
		}

		foreach (Collider2D col in toPowerCols) {
			if (col != null) {	// toPowerCols.Length is 128
				if (col.gameObject.GetComponent<IPowerable> () != null && !powerSources.Contains (col.gameObject)) {
					if (col.gameObject.name != gameObject.name && !goImPowering.Contains (col.gameObject)) {
						goImPowering.Add (col.gameObject);
					}
					col.gameObject.GetComponent<IPowerable> ().AddPower (gameObject, color, powerOrigin);
				}
			}
		}

	}

	public void RemovePower (GameObject source, List <string> _powerOrigins) {
		foreach (string powerOrigin in _powerOrigins.ToArray()) {
			RemovePower (source, powerOrigin);
		}

	}

	public void RemovePower(GameObject source, string powerOrigin) {

		//if (!powerOrigins.Contains (powerOrigin) ) { 
		if (!powerSources.Contains(source) ) { 
			return;
		}

		powerSources.Remove (source);
		powerOrigins.Remove (powerOrigin);

		// remove power from all connected IActor
		for (int i = 0; i < goImPowering.Count; i++) {
			goImPowering [i].GetComponent<IPowerable> ().RemovePower (gameObject, powerOrigin);
		}

		// turn self offline if online
		if (powerSources.Count == 0 && powerSources.Count + 1 == 1) {
			GetComponent<SpriteRenderer> ().color = startColor;

			goImPowering.Clear ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<IPowerable> () != null && powerOrigins.Count > 0 && !goImPowering.Contains (other.gameObject)) {
			other.GetComponent<IPowerable> ().AddPower (gameObject, "#"+ColorUtility.ToHtmlStringRGBA(GetComponent<SpriteRenderer> ().color) , powerOrigins);
			goImPowering.Add (other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<IPowerable> () != null && powerOrigins.Count > 0  && goImPowering.Contains(other.gameObject)) {
			// TODO: dont just send the last powerOrigin in List, send the right one..
			other.GetComponent<IPowerable> ().RemovePower (gameObject, powerOrigins);
			goImPowering.Remove (other.gameObject);
		}
	}

}
