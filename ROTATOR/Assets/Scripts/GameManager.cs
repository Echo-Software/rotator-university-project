using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Public variables
	public GameObject[] playerShipSelection;
	public GameObject[] checkpoints;
	public Transform[] startingGrid = new Transform[4];
	public int numberOfPlayers = 4;

	// Private variables
	private float[] lapTimers = new float[4];
	private float[] totalTimers = new float[4];
	[SerializeField]
	private float[] player1Laps, player2Laps, player3Laps, player4Laps;


	// Use this for initialization
	void Start() {
		player1Laps = new float[3];
		player2Laps = new float[3];
		player3Laps = new float[3];
		player4Laps = new float[3];
		playerShipSelection = new GameObject[numberOfPlayers];
		GameObject[] temp = GameObject.FindGameObjectsWithTag ("Player");

		// Arrange all ships by player control
		for (int count = 0; count < numberOfPlayers; count++){
			for (int count2 = 0; count2 < numberOfPlayers; count2++) {
				if (temp [count2].GetComponent<VehicleControl> ().controllingPlayer == count+1) {
					playerShipSelection[count] = temp [count2];
				}
			}
		}

		// Sort ship starting grid positions based on the player number
		for (int count = 0; count < numberOfPlayers; count++) {
			playerShipSelection [count].transform.position = startingGrid [count].position;
		}

	}
	
	// Update is called once per frame
	void Update() {
		RunTimers ();
	}

	// Keeps lap and total timers running based on number of players
	private void RunTimers(){
		if (numberOfPlayers == 2) {
			lapTimers [0] += Time.deltaTime;
			lapTimers [1] += Time.deltaTime;
			totalTimers [0] += Time.deltaTime;
			totalTimers [1] += Time.deltaTime;			
		}
		if (numberOfPlayers == 3) {
			lapTimers [0] += Time.deltaTime;
			lapTimers [1] += Time.deltaTime;
			lapTimers [2] += Time.deltaTime;
			totalTimers [0] += Time.deltaTime;
			totalTimers [1] += Time.deltaTime;	
			totalTimers [2] += Time.deltaTime;
		}
		if (numberOfPlayers == 4) {
			lapTimers [0] += Time.deltaTime;
			lapTimers [1] += Time.deltaTime;
			lapTimers [2] += Time.deltaTime;
			lapTimers [3] += Time.deltaTime;
			totalTimers [0] += Time.deltaTime;
			totalTimers [1] += Time.deltaTime;
			totalTimers [2] += Time.deltaTime;
			totalTimers [3] += Time.deltaTime;
		}
	}

	// Returns the requested timer type for use in the interface display for a specified player
	public float ReturnTime(int playerNumber, string type){
		if (type == "Lap") {
			return lapTimers [playerNumber];
		} 
		else if (type == "Total") {
			return totalTimers [playerNumber];
		} 
		else {
			return 0;
		}
	}

	// Stores the current lap for the specified player in an array and then resets the lap timer for 0 for the next lap
	public void NewLap(int player, int lapCount){
		if (lapCount < 4) {
			if (player == 1) {
				player1Laps [lapCount - 1] = lapTimers [player - 1];
			}
			if (player == 2) {
				player2Laps [lapCount - 1] = lapTimers [player - 1];
			}
			if (player == 3) {
				player3Laps [lapCount - 1] = lapTimers [player - 1];
			}
			if (player == 4) {
				player4Laps [lapCount - 1] = lapTimers [player - 1];
			}
			lapTimers [player - 1] = 0;
		}
	}

}
