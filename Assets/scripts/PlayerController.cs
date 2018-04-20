using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	private Rigidbody2D rb;

	public float runningSpeed;
	public float rotationSpeed;
	public GameObject playerCam;
	//public List<string> entities;
	//public List<string> powerOrigin;	// list of power sources this player got its electricity from
	public Dictionary<string,string> actions;

	void Start () {
		//startColor = GetComponent<SpriteRenderer> ().color;

		rb = GetComponent<Rigidbody2D>();
	//	entities = new List<string> { "player", "pressure"};
	//	actions = new Dictionary<string, string> (){{"poweron", "false"},{"mass", rb.mass.ToString()},{"color",""}};

		if (GameManager.instance.gameMode == "single") {
			playerCam.SetActive (false);
			if (gameObject.tag == "Player") {
				GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ().player = gameObject;
			}
		}

		if (GameManager.instance.gameMode == "coop") {
			playerCam.SetActive (false);
			GameObject cam;
			if (gameObject.tag == "Player") {
				cam = GameObject.FindGameObjectWithTag ("MainCamera");
				cam.GetComponent<CameraController> ().enabled = false;
				cam.GetComponent<MultipleTargetCamera> ().enabled = true;
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
				
			if (GameManager.instance.gameMode == "coop" && gameObject.tag == "Player2") {
				// Player movement
				Vector2 move = new Vector2 ();
				move.x = Input.GetAxis ("Horizontal_Player2");
				move.y = Input.GetAxis ("Vertical_Player2");
				move = move.normalized * Time.deltaTime * runningSpeed;

				rb.AddForce (move);

				// Player rotation

				//if (move != Vector2.zero) {
				if (new Vector2 (Input.GetAxis ("Horizontal_Player2"), Input.GetAxis ("Vertical_Player2")).magnitude >= .2f) {  // only rotate if we have player movement
					float angle = Mathf.Atan2 (move.y, move.x) * Mathf.Rad2Deg;
					transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.AngleAxis (angle + -90, Vector3.forward), Time.deltaTime * rotationSpeed);
				}

			} else {
				
				// Player movement
				Vector2 move = new Vector2 ();
				move.x = Input.GetAxis ("Horizontal");
				move.y = Input.GetAxis ("Vertical");
				move = move.normalized * Time.deltaTime * runningSpeed;
				
				rb.AddForce (move);

				// Player rotation

				//if (move != Vector2.zero) {
				if (new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).magnitude >= .2f) {  // only rotate if we have player movement
					float angle = Mathf.Atan2 (move.y, move.x) * Mathf.Rad2Deg;
					transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.AngleAxis (angle + -90, Vector3.forward), Time.deltaTime * rotationSpeed);
				}
			}
		}
	}
		
}
