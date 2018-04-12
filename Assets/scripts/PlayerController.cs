using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;
	public float runningSpeed;
	public float rotationSpeed;
	public GameObject playerCam;
	public List<string> entities;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		entities = new List<string> { "player", "pressure", "conducting"};

		if (GameManager.instance.gameMode == "single") {
			playerCam.SetActive (false);
			if (gameObject.tag == "Player") {
				GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ().player = gameObject;
			}
		}
	}

	void FixedUpdate () {

		// Player movement 

		rb.velocity = Vector2.zero;
		if (GameManager.instance.isPlayerAbleToInteract && !GameManager.instance.isGameLost )
		{
			if (GameManager.instance.gameMode == "single" && gameObject.tag != GameManager.instance.activePlayerTag) {
				// player is in single player mode and other character is active
				return;
			}
				
			// Player movement

			Vector2 move = new Vector2();
			move.x = Input.GetAxis("Horizontal");
			move.y = Input.GetAxis("Vertical");
			move = move.normalized * Time.deltaTime * runningSpeed;
				
			rb.AddForce(move);

			// Player rotation

			//if (move != Vector2.zero) {
			if (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude >= .2f) {  // only rotate if we have player movement
				float angle = Mathf.Atan2 (move.y, move.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis (angle + -90, Vector3.forward), Time.deltaTime * rotationSpeed);
			}
		}
	}
		
	public bool hasEntity(string name) {
		return entities.Contains (name);
	}


	void OnTriggerEnter2D(Collider2D other) {
		other.GetComponent<ITrigger> ().Trigger (entities, rb.mass);
	//	if (other.gameObject.CompareTag("PressureButton") && entities.Contains("pressure")) {
	//		other.gameObject.GetComponent<PressureButton>().ChangePressure(gameObject.GetComponent<Rigidbody2D>().mass);
	//	}
	}

	void OnTriggerExit2D(Collider2D other) {
		other.GetComponent<ITrigger> ().StopTrigger (entities, rb.mass);
	//	if (other.gameObject.CompareTag("PressureButton") && entities.Contains("pressure")) {
	//		other.gameObject.GetComponent<PressureButton>().ChangePressure(-gameObject.GetComponent<Rigidbody2D>().mass);
	//	}
	}

}
