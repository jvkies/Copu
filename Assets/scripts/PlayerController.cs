using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IActor {

	private Color startColor;
	private Rigidbody2D rb;
	public List<GameObject> goImPowering;
	public List<GameObject> powerSources;
	public List<string> powerOrigins;				// list of unique power origins (button, player..)

	public int powerSourcesCount;		// amount of connected powersources
	public float runningSpeed;
	public float rotationSpeed;
	public GameObject playerCam;
	public List<string> entities;
	public List<string> powerOrigin;	// list of power sources this player got its electricity from
	public Dictionary<string,string> actions;

	void Start () {
		startColor = GetComponent<SpriteRenderer> ().color;
		goImPowering = new List<GameObject> {};
		powerSources = new List<GameObject> {};
		powerOrigins = new List<string> {};

		rb = GetComponent<Rigidbody2D>();
		entities = new List<string> { "player", "pressure"};
		actions = new Dictionary<string, string> (){{"poweron", "false"},{"mass", rb.mass.ToString()},{"color",""}};

		if (gameObject.tag == "Player") {
			entities.Add ("conductive");
		}

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
		if (other.GetComponent<IActor> () != null) {
			other.GetComponent<IActor> ().Trigger (actions, entities, gameObject);
			if (actions.ContainsKey ("poweron") && actions ["poweron"] != "false" && !goImPowering.Contains (other.gameObject)) {
				goImPowering.Add (other.gameObject);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<IActor> () != null) {
			Dictionary<string, string> _actions2 = LeaveActions ();
				other.GetComponent<IActor> ().Trigger (_actions2, entities, gameObject);
			if (actions.ContainsKey ("poweron") && actions ["poweron"] != "false") {
				goImPowering.Remove (other.gameObject);
			}

		}
	}
		

	public void Trigger(Dictionary<string, string> _actions, List<string> _entities, GameObject source) {
		if (entities.Contains ("conductive") && _actions.ContainsKey("poweron") && _actions["poweron"] != "false") {
			ConductElectricity (true, _actions ["color"], _actions, source);
		}
		if (entities.Contains ("conductive") && _actions.ContainsKey("poweroff") && _actions["poweroff"] != "false" && powerSources.Contains(source)) {
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
		int colCount = Physics2D.OverlapCollider (GetComponent<PolygonCollider2D>(),cf2d, toPowerCols);

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

	private void RemovePower(GameObject source, string powerOrigin) {

		powerSources.Remove (source);
		powerOrigins.Remove (powerOrigin);

		// remove power from all connected IActor
		for (int i = 0; i < goImPowering.Count; i++) {
			goImPowering [i].GetComponent<IActor> ().Trigger (new Dictionary<string, string> () { { "poweroff", powerOrigin } }, entities, gameObject);
		}

		// turn self offline if online
		if (powerSources.Count == 0 && powerSources.Count + 1 == 1) {
			GetComponent<SpriteRenderer> ().color = startColor;
			actions ["poweron"] = "false";

			goImPowering.Clear ();
		}
	}


	// actions to send when disengaging from an actor
	private Dictionary<string, string> LeaveActions() {
		Dictionary<string, string> leaveActions = new Dictionary<string, string> ();
		leaveActions.Add("mass", (-rb.mass).ToString ());
		if (actions.ContainsKey("poweron") && actions ["poweron"] != "false") {
			leaveActions.Add ("poweroff", powerOrigins [powerOrigins.Count-1]);
		}

		return leaveActions;

	}

}
