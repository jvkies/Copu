using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1Manager : MonoBehaviour {

	public GameObject swapX;

	// Use this for initialization
	void Start () {
		if (GameManager.instance.gameMode == "single") {
			swapX.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
