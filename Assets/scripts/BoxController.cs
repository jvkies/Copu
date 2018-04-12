using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {

	Rigidbody2D rb;
	public List<string> entities;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		entities = new List<string> { "pressure"};

		//		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ().player = gameObject;
	}

	void OnTriggerEnter2D(Collider2D other) {
		other.GetComponent<ITrigger> ().Trigger (entities, rb.mass);
	}

	void OnTriggerExit2D(Collider2D other) {
		other.GetComponent<ITrigger> ().StopTrigger (entities, rb.mass);
	}

}
