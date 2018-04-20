using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float transitionDuration = 0.5f;
	public AnimationCurve cameraTransition;

	void Start () {
		//player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void Update () {
		if (player != null) {
			
			transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
		}
	}

	// this is for single player
	public void SwitchCameraFocus(string newPlayerTag) {
		if (GameManager.instance.isPlayerAbleToInteract) {
			GameObject newTarget = GameObject.FindGameObjectWithTag (newPlayerTag);
			player = null;
			StopCoroutine ("SmoothTransform");
			StartCoroutine (SmoothTransition (newTarget));
		}
	}


	IEnumerator SmoothTransition(GameObject target)
	{
		float t = 0.0f;
		Vector3 startingPos = transform.position;
		while (t < 1.0f) {
			t += Time.deltaTime * (Time.timeScale / transitionDuration);

			Vector3 newCamPos = new Vector3 (target.transform.position.x, target.transform.position.y, transform.position.z);

			transform.position = Vector3.Lerp (startingPos, newCamPos , cameraTransition.Evaluate(t));
			yield return 0;
		}

		player = target;
	}

}
