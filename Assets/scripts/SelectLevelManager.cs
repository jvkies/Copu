using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelManager : MonoBehaviour {

	private int highestLevel;
	public Transform levelsPanel;

	// Use this for initialization
	void Start () {
		highestLevel = PlayerPrefs.GetInt ("highestLevel", 0);

		UnlockLevels ();
	}

	// all levels are locked on default, this unlocks the ones the player cleared already
	private void UnlockLevels() {
		for (int i = 0; i <= levelsPanel.childCount; i++) { 
			if (i <= highestLevel) {
				//levelsPanel.GetChild (i).gameObject.GetComponent<Button> ().enabled = true;
				levelsPanel.GetChild (i).GetChild (3).gameObject.SetActive (false);
			}
		}

	}

	public void SelectLevel(int levelNumber) {
		if (levelNumber <= highestLevel+1) {
			GameManager.instance.activeLevel = levelNumber;
			GameManager.instance.LoadScene ("Level" + levelNumber.ToString ());
		}
	}

}
