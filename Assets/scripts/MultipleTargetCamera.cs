using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour {

	//public List<Transform> targets;
	public Transform player1;
	public Transform player2;

	public Vector3 offset; 
	public float smoothTime = 0.5f;

	public float minZoom = 40f;
	public float maxZoom = 10f;
	public float zoomLimiter = 50f;

	private Vector3 velocity;
	//private Camera cam;

	void Start () {
		//cam = GetComponent<Camera> ();
		player1 = GameObject.FindWithTag ("Player").transform;
		player2 = GameObject.FindWithTag ("Player2").transform;
	}
	
	void LateUpdate () {
		//if (targets.Count == 0) {
		//	return;
		//}

		Move ();
		//Zoom ();
	}

	void Zoom () {
		//float newZoom = Mathf.Lerp (maxZoom, minZoom, GetGreatestDistance () / zoomLimiter);
	}

	void Move() {
		Vector3 centerPoint = GetCenterPoint ();
		Vector3 newPosition = centerPoint + offset;
		Vector3 smoothPosition = Vector3.SmoothDamp (transform.position, newPosition, ref velocity, smoothTime);
		smoothPosition.z = -10;
		transform.position = smoothPosition;
	}

	float GetGreatestDistance() {
		//var bounds = new Bounds (targets [0].position, Vector3.zero);
		var bounds = new Bounds (player1.position, Vector3.zero);
		bounds.Encapsulate(player2.position);

		//for (int i = 0; i < targets.Count; i++) {
		//	bounds.Encapsulate(targets[i].position);
		//}

		return bounds.size.x;
	}

	Vector3 GetCenterPoint() {
	//	if (targets.Count == 1) {
	//		return targets [0].position;
	//	}

		var bounds = new Bounds (player1.position, Vector3.zero);
		bounds.Encapsulate(player2.position);

		//var bounds = new Bounds (targets [0].position, Vector3.zero);
		//for (int i = 0; i < targets.Count; i++) {
		//	bounds.Encapsulate(targets[i].position);
		//}

		return bounds.center;
	}
}
