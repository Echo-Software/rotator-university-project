using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipSelection : MonoBehaviour {

	// Private variables
	private MenuManager menu;
	private int[] playerShipSelection = new int[4];
	private bool[] playerActive = new bool[4];

	// Public variables
	public Image[] shipSelection;
	public Sprite[] selectionImages;
	public Slider playerNumberSlider;

	// Use this for initialization
	void Start () {
		menu = GameObject.Find ("MenuManager").GetComponent<MenuManager> ();
		shipSelection [0].sprite = selectionImages [1];
		playerShipSelection [0] = 1;
		playerActive [0] = true;
		shipSelection [1].sprite = selectionImages [2];
		playerShipSelection [1] = 2;
		playerActive [1] = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void CheckPlayerNumber(){
		if (playerNumberSlider.value == 2) {
			shipSelection [2].sprite = selectionImages [0];
			playerShipSelection [2] = 0;
			playerActive [2] = false;
			shipSelection [3].sprite = selectionImages [0];
			playerShipSelection [3] = 0;
			playerActive [3] = false;
		}
		if (playerNumberSlider.value == 3) {
			shipSelection [2].sprite = selectionImages [3];
			playerShipSelection [2] = 3;
			playerActive [2] = true;
			shipSelection [3].sprite = selectionImages [0];
			playerShipSelection [3] = 0;
			playerActive [3] = false;
		}
		if (playerNumberSlider.value == 4) {
			shipSelection [3].sprite = selectionImages [4];
			playerShipSelection [3] = 4;
			playerActive [3] = true;
		}	
	}

	public void PrevShipP1(){
		if (playerActive [0]) {
			if (playerShipSelection [0] > 1) {
				playerShipSelection [0]--;
				shipSelection [0].sprite = selectionImages [playerShipSelection [0]];
			} 
			else {
				playerShipSelection [0] = 4;
				shipSelection [0].sprite = selectionImages [4];
			}
		}
	}

	public void NextShipP1(){
		if (playerActive [0]) {
			if (playerShipSelection [0] < 4) {
				playerShipSelection [0]++;
				shipSelection [0].sprite = selectionImages [playerShipSelection [0]];
			} 
			else {
				playerShipSelection [0] = 1;
				shipSelection [0].sprite = selectionImages [1];
			}
		}
	}

	public void PrevShipP2(){
		if (playerActive [1]) {
			if (playerShipSelection [1] > 1) {
				playerShipSelection [1]--;
				shipSelection [1].sprite = selectionImages [playerShipSelection [1]];
			} 
			else {
				playerShipSelection [1] = 4;
				shipSelection [1].sprite = selectionImages [4];
			}
		}
	}

	public void NextShipP2(){
		if (playerActive [1]) {
			if (playerShipSelection [1] < 4) {
				playerShipSelection [1]++;
				shipSelection [1].sprite = selectionImages [playerShipSelection [1]];
			} 
			else {
				playerShipSelection [1] = 1;
				shipSelection [1].sprite = selectionImages [1];
			}
		}
	}

	public void PrevShipP3(){
		if (playerActive [2]) {
			if (playerShipSelection [2] > 1) {
				playerShipSelection [2]--;
				shipSelection [2].sprite = selectionImages [playerShipSelection [2]];
			} 
			else {
				playerShipSelection [2] = 4;
				shipSelection [2].sprite = selectionImages [4];
			}
		}
	}

	public void NextShipP3(){
		if (playerActive [2]) {
			if (playerShipSelection [2] < 4) {
				playerShipSelection [2]++;
				shipSelection [2].sprite = selectionImages [playerShipSelection [2]];
			} 
			else {
				playerShipSelection [2] = 1;
				shipSelection [2].sprite = selectionImages [1];
			}
		}
	}

	public void PrevShipP4(){
		if (playerActive [3]) {
			if (playerShipSelection [3] > 1) {
				playerShipSelection [3]--;
				shipSelection [3].sprite = selectionImages [playerShipSelection [3]];
			} 
			else {
				playerShipSelection [3] = 4;
				shipSelection [3].sprite = selectionImages [4];
			}
		}
	}

	public void NextShipP4(){
		if (playerActive [3]) {
			if (playerShipSelection [3] < 4) {
				playerShipSelection [3]++;
				shipSelection [3].sprite = selectionImages [playerShipSelection [3]];
			} 
			else {
				playerShipSelection [3] = 1;
				shipSelection [3].sprite = selectionImages [1];
			}
		}
	}

	public void StartRace(){
		menu.SetPlayerNumber (Mathf.RoundToInt(playerNumberSlider.value));

		for (int count = 0; count < playerNumberSlider.value; count++) {
			menu.SetShipSelection (playerShipSelection [count] - 1, count + 1);
		}

		menu.PlayRaceLoop ();
		SceneManager.LoadScene("Track", LoadSceneMode.Single);
	}

}