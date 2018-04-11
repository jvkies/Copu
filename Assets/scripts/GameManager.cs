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
	//public static bool isOnSceneLoadedCalled;			// prevent function OnSceneLoaded be called twice (http://www.wenyu-wu.com/blogs/unity-when-using-onlevelwasloaded-and-dontdestroyonload-together/)

	public bool isGameLost = false;
	public bool isPlayerAbleToInteract = true;
	public string activePlayerTag = "Player";
	public CameraController cameraC;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		SceneManager.sceneLoaded += OnSceneLoaded; 		// using a delegate here, adding our own function OnSceneLoaded to get event calles from sceneLoaded
		//isOnSceneLoadedCalled = false;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene ().name == "Game") {
			//cameraC = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController>();
			//InitNewGame ();
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
		if (SceneManager.GetActiveScene ().name == "Game") {
			if (Input.GetKeyDown (KeyCode.X)) {
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

	public void StartButton () {
		// Can be continue where player left, later
		NewGame ();
	}

	public void NewGame() {
		InitNewGame ();
		LoadScene ("Game");
	}

	private void InitNewGame() {
	}

	public void LoadScene(string sceneName) {
		Time.timeScale = 1;
		SceneManager.LoadScene (sceneName);
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
