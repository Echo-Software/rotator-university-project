﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Public variables
	public GameObject[] playerShipSelection;
	public GameObject[] checkpoints;
	public Transform[] startingGrid = new Transform[4];
	public int numberOfPlayers;
	public bool raceStarted = false;

	// Private variables
	private InterfaceManager im;
	private MenuManager menu;
	private float[] lapTimers = new float[4];
	private float[] totalTimers = new float[4];
	[SerializeField]
	private float[] player1Laps, player2Laps, player3Laps, player4Laps;


	// Use this for initialization
	void Start() {
		im = GameObject.Find ("InterfaceManager").GetComponent<InterfaceManager> ();
		menu = GameObject.Find ("MenuManager").GetComponent<MenuManager> ();
		numberOfPlayers = menu.GetPlayerNumber ();
		player1Laps = new float[3];
		player2Laps = new float[3];
		player3Laps = new float[3];
		player4Laps = new float[3];
		// playerShipSelection = menu.GetShipSelection();
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

		// Start the countdown for the start of the race
		StartRace();
	}
	
	// Update is called once per frame
	void Update() {
		RunTimers ();
		ShipPosition ();
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

	// Sorts the player position based on lap count, checkpoint count and distance from next checkpoint
	private void ShipPosition(){
		// All players start at "last" place, based on the number of players
		int tempPosition = numberOfPlayers;

		// Start move players up in position based on a series of checks within a series of nested loop/if statements
		for (int count = 0; count < numberOfPlayers; count++) {
			int count2;
			for (count2 = 0; count2 < numberOfPlayers; count2++) {
				// Stop the player from checking against themselves
				if (count != count2){
					// Check if the player has a higher lap count to determine position
					if (playerShipSelection [count].GetComponent<VehicleControl> ().lapCount > playerShipSelection [count2].GetComponent<VehicleControl> ().lapCount) {
						tempPosition--;
					}
					// If the player has the same lap count as the player being checked against, move onto the next set of tests
					else if (playerShipSelection [count].GetComponent<VehicleControl> ().lapCount == playerShipSelection [count2].GetComponent<VehicleControl> ().lapCount) {
						// Check if the player is further along the lap checkpoints to determine position
						if(playerShipSelection [count].GetComponent<VehicleControl> ().nextCheckpoint > playerShipSelection [count2].GetComponent<VehicleControl> ().nextCheckpoint) {
							tempPosition--;
						}
						// If the player is on the same checkpoint as the player being checked against, move onto the next set of tests
						else if(playerShipSelection [count].GetComponent<VehicleControl> ().nextCheckpoint == playerShipSelection [count2].GetComponent<VehicleControl> ().nextCheckpoint) {
							// Check the distance to the next checkpoint against the player being checked against to determine position
							if (Vector3.Distance (playerShipSelection [count].transform.position, checkpoints [playerShipSelection [count].GetComponent<VehicleControl> ().nextCheckpoint-1].transform.position) <
								Vector3.Distance (playerShipSelection [count2].transform.position, checkpoints [playerShipSelection [count2].GetComponent<VehicleControl> ().nextCheckpoint-1].transform.position)) {							
								tempPosition--;
							}
						}
					}
				}
			}

			// After all the tests have been run through set the players position to the result of all the checks and then reset the tempPosition and count2 variables for future checks
			playerShipSelection [count].GetComponent<VehicleControl> ().currentPosition = tempPosition;
			tempPosition = numberOfPlayers;
			count2 = 0;
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

	private void StartRace(){
		StartCoroutine (im.Countdown ());
	}

}
