using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Results : MonoBehaviour {

	// Private variables
	private MenuManager menu;
	private int[] finalPositions;
	private float[] p1Laps, p2Laps, p3Laps, p4Laps;

	// Public variables
	public GameObject[] resultDisplay;

	// Use this for initialization
	void Start () {
		menu = GameObject.Find ("MenuManager").GetComponent<MenuManager> ();
		finalPositions = menu.GetFinalPositions ();

		// Initialize correct results display based on player number
		if (menu.GetPlayerNumber () == 2) {
			resultDisplay [0].SetActive (true);
			resultDisplay [1].SetActive (true);
		}
		else if (menu.GetPlayerNumber () == 3) {
			resultDisplay [0].SetActive (true);
			resultDisplay [1].SetActive (true);
			resultDisplay [2].SetActive (true);
		}
		else if (menu.GetPlayerNumber () == 4) {
			resultDisplay [0].SetActive (true);
			resultDisplay [1].SetActive (true);
			resultDisplay [2].SetActive (true);
			resultDisplay [3].SetActive (true);
		}

		for (int count = 0; count < menu.GetPlayerNumber (); count++) {
			float[] tempLap = menu.GetLapTimes (count + 1);
			resultDisplay [count].transform.GetChild (0).GetComponent<Text> ().text = "Position: " + finalPositions [count].ToString ();
			resultDisplay [count].transform.GetChild (1).GetComponent<Text> ().text = "Lap 1: " + FormatTime (tempLap [0]);
			resultDisplay [count].transform.GetChild (2).GetComponent<Text> ().text = "Lap 2: " + FormatTime (tempLap [1]);
			resultDisplay [count].transform.GetChild (3).GetComponent<Text> ().text = "Lap 3: " + FormatTime (tempLap [2]);
		}
	}

	private string FormatTime(float timer){
		int minutes = Mathf.FloorToInt(timer / 60);
		int seconds = Mathf.FloorToInt(timer % 60);
		int milliseconds = Mathf.FloorToInt ((timer * 100) % 100);

		return minutes.ToString("00") + ":" + seconds.ToString("00") + "." + milliseconds.ToString("00");
	}

}
