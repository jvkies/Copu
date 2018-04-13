using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {

	private Rigidbody2D rb;
	private Dictionary<string,string> actions;
	public List<string> entities;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		actions = new Dictionary<string,string> (){ { "mass", rb.mass.ToString () } };
		entities = new List<string> { "pressure"};

		//		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ().player = gameObject;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<IActor> () != null) {
			other.GetComponent<IActor> ().Trigger (actions, entities, gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<IActor> () != null) {
			other.GetComponent<IActor> ().Trigger (new Dictionary<string,string> (){ { "mass", (-rb.mass).ToString () } }, entities, gameObject);
		}

	}
		
}
