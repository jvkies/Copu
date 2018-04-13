﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinZone : MonoBehaviour, IActor {

	private int playerCounter;

	public float loadDelayTime=3;
	public string transferToSceneName;
	public GameObject winPanel;
	public GameObject waitingForPlayerPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Trigger(Dictionary<string, string> actions, List<string> entities, GameObject source) {
		if (entities.Contains ("player") && actions.ContainsKey("mass") && float.Parse(actions["mass"]) > 0) {
			ChangePlayerInZone (1);
		}
		if (entities.Contains ("player") && actions.ContainsKey("mass") && float.Parse(actions["mass"]) < 0) {
			ChangePlayerInZone (-1);
		}
	}


	public void ChangePlayerInZone(int amount) {
		playerCounter += amount;

		if (playerCounter == 0) {
			waitingForPlayerPanel.SetActive (false);
		}

		if (playerCounter == 1) {
			waitingForPlayerPanel.SetActive (true);
		}

		if (playerCounter == 2) {
			GameManager.instance.isPlayerAbleToInteract = false;
			waitingForPlayerPanel.SetActive (false);
			winPanel.SetActive(true);

			Debug.Log ("You win");

			Invoke ("LoadSceneLazy", loadDelayTime);
		}
	}

	public void LoadSceneLazy() {
		if (transferToSceneName != "") {
			GameManager.instance.LoadScene (transferToSceneName);
		}
	}

}
