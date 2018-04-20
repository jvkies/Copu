using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : MonoBehaviour {

	private Rigidbody2D rb;
	//private Dictionary<string,string> actions;
	//public List<string> entities;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		//actions = new Dictionary<string,string> (){ { "mass", rb.mass.ToString () } };
		//entities = new List<string> { "pressure"};
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<IPressureable> () != null) {
			other.GetComponent<IPressureable> ().AddPressure (rb.mass);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<IPressureable> () != null) {
			//other.GetComponent<IPressureable> ().Trigger (new Dictionary<string,string> (){ { "mass", (-rb.mass).ToString () } }, entities, gameObject);
			other.GetComponent<IPressureable> ().RemovePressure (rb.mass);
		}

	}

}
