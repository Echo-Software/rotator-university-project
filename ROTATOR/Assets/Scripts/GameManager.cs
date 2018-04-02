using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Public variables
	public GameObject[] playerShipSelection;
	public GameObject[] checkpoints;
	public int numberOfPlayers = 4;
	private float[] lapTimers = new float[4];
	private float[] totalTimers = new float[4];
	[SerializeField]
	private float[] p1laps, p2laps, p3laps, p4laps;

	// Private variables


	// Use this for initialization
	void Start() {
		p1laps = new float[3];
		p2laps = new float[3];
		p3laps = new float[3];
		p4laps = new float[3];
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

	}
	
	// Update is called once per frame
	void Update() {
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

	public void NewLap(int player, int lapCount){
		if (lapCount < 4) {
			if (player == 1) {
				p1laps [lapCount - 1] = lapTimers [player - 1];
			}
			if (player == 2) {
				p2laps [lapCount - 1] = lapTimers [player - 1];
			}
			if (player == 3) {
				p3laps [lapCount - 1] = lapTimers [player - 1];
			}
			if (player == 4) {
				p4laps [lapCount - 1] = lapTimers [player - 1];
			}
			lapTimers [player - 1] = 0;
		}
	}

}
