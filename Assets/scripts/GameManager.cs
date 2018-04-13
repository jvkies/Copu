using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public static bool isOnSceneLoadedCalled;			// prevent function OnSceneLoaded be called twice (http://www.wenyu-wu.com/blogs/unity-when-using-onlevelwasloaded-and-dontdestroyonload-together/)

	private CameraController cameraC;

	public bool isGameLost = false;
	public bool isPlayerAbleToInteract = true;
	public string gameMode = "single"; 					// "single" for single player, "coop" for local coop, "lan" for lan multiplayer
	public string activePlayerTag = "Player";
	public GameObject player1Prefab;
	public GameObject player2Prefab;
	public GameObject networkManager;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		SceneManager.sceneLoaded += OnSceneLoaded; 		// using a delegate here, adding our own function OnSceneLoaded to get event calles from sceneLoaded
		isOnSceneLoadedCalled = false;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (!isOnSceneLoadedCalled) {
			isOnSceneLoadedCalled = true;				// i dont know why, but this sometimes get called twice, hence the workaround

			if (SceneManager.GetActiveScene ().name != "MainMenu") {
				if (gameMode == "single") {
					cameraC = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ();
					SpawnPlayer ();
				}
				//InitNewGame ();
			}
		}

		//if (!isOnSceneLoadedCalled) {
			// if we start in the main menu, certain object cannot be found
	//		try {
	//			InitNewGame();

				//isOnSceneLoadedCalled = true;
	//		} catch {
	//		} 
	//	}

	}


	void Update() {
		if (SceneManager.GetActiveScene ().name == "MainMenu") {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();					
			}
		}		
		if (SceneManager.GetActiveScene ().name != "MainMenu") {
			if (gameMode == "single" && Input.GetKeyDown (KeyCode.X)) {
				SwitchActivePlayer ();
			}
		}
	}

	public void SwitchActivePlayer() {
		if (activePlayerTag == "Player") {
			activePlayerTag = "Player2";
		} else {
			activePlayerTag = "Player";
		}
	
		cameraC.SwitchCameraFocus (activePlayerTag);
	}

	public void SpawnPlayer() {
		GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag ("Respawn");

		if (spawnpoints.Length != 2) {
			Debug.Log ("Error: no two spawnpoints found (with tag 'Respawn')");
			return;
		}

		Instantiate (player1Prefab, spawnpoints [0].transform.position, Quaternion.identity);
		Instantiate (player2Prefab, spawnpoints [1].transform.position, Quaternion.identity);

	}

	public void StartButton (string mode) {
		gameMode = mode;
		// Can be continue where player left, later
		NewGame ();
	}

	public void NewGame() {
		InitNewGame ();
		SceneManager.LoadScene ("Level1");
	}

	private void InitNewGame() {
	}

	public void LoadScene(string sceneName) {
		Time.timeScale = 1;
		isPlayerAbleToInteract = true;

		if (gameMode == "lan") {
			NetworkManagerCustom.singleton.ServerChangeScene(sceneName);
		} else {
			SceneManager.LoadScene (sceneName);
		}
	}

	public void GameOver() {
		LoadScene ("GameOver");
	}

	public void MainMenuScene() {
		LoadScene ("MainMenu");
	}		

	public void ExitGame() {
		Application.Quit ();
	}


}
