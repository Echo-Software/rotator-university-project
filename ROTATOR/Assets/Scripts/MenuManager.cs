﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	// Private variables
	private AudioSource audioSource;
	private int numberOfPlayers = 4;
	[SerializeField]
	private GameObject[] selectedShips;
	private static MenuManager instanceReference;
	private Button[] buttons = new Button[4];
	private int[] finalPositions;

	// Public variables
	public bool buttonCheck = false;
	public AudioClip[] music;
	public GameObject[] selectableShips = new GameObject[4];
	public Image brightnessOverlay;
	public float[] player1Laps, player2Laps, player3Laps, player4Laps;

	void Awake(){
		if (instanceReference == null) {
			instanceReference = this;
			DontDestroyOnLoad (this.gameObject);
		} 
		else {
			Destroy (this.gameObject);
		}
	}

	void Start(){
		audioSource = this.gameObject.GetComponent<AudioSource> ();
		brightnessOverlay = this.gameObject.GetComponentInChildren<Canvas> ().GetComponent<Image> ();

		// Set the brightness to full by default at the start
		brightnessOverlay.color = new Color (0, 0, 0, 0);
		ResetButtons ();
	}

	void Update(){
		if (buttonCheck) {
			ResetButtons ();
		}
	}

	public void SetPlayerNumber(int players){
		numberOfPlayers = players;
		selectedShips = new GameObject[players];
	}

	public int GetPlayerNumber(){
		return numberOfPlayers;
	}

	public void SetShipSelection(int shipSelection, int controllingPlayer){
		selectedShips [controllingPlayer - 1] = selectableShips [shipSelection];
	}

	public GameObject[] GetShipSelection(){
		return selectedShips;
	}

	public void SetLapTimes(float[] lapTimes, int player){
		if (player == 1){
			player1Laps = lapTimes;
		}
		if (player == 2){
			player2Laps = lapTimes;
		}
		if (player == 3){
			player3Laps = lapTimes;
		}	
		if (player == 4){
			player4Laps = lapTimes;
		}	
	}

	public float[] GetLapTimes(int player){
		if (player == 1){
			return player1Laps;
		}
		if (player == 2){
			return player2Laps;
		}
		if (player == 3){
			return player3Laps;
		}	
		if (player == 4){
			return player4Laps;
		}

		return null;
	}

	public void SetFinalPositions(int[] finalPos){
		finalPositions = finalPos;
	}

	public int[] GetFinalPositions(){
		return finalPositions;
	}

	public void PlayMenuLoop(){
		audioSource.Stop ();
		audioSource.clip = music [0];
		audioSource.Play ();
		audioSource.loop = true;		
	}

	public void PlayRaceLoop(){		
		audioSource.Stop ();
		audioSource.clip = music [1];
		audioSource.Play ();
		audioSource.loop = true;
	}

	public void ResetButtons(){
		StartCoroutine (TimedButtonCheck(0.15f));
		buttonCheck = false;
	}

	public void SetBrightness(float alpha){
		brightnessOverlay.color = new Color (0, 0, 0, alpha);
	}

	public float GetBrightness(){
		return brightnessOverlay.color.a;
	}

	public void SetVolume(float volume){
		AudioListener.volume = volume;
	}

	public float GetVolume(){
		return AudioListener.volume;
	}

	// All menu selection options
	public void NewRace() {				
		SceneManager.LoadScene("NewRace", LoadSceneMode.Single);
	}

	public void Instructions() {
		SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
	}

	public void Settings() {
		SceneManager.LoadScene("Settings", LoadSceneMode.Single);
	}

	public void ExitGame(){
		Application.Quit ();
	}

	public IEnumerator TimedButtonCheck(float time){
		yield return new WaitForSeconds (time);

		// Get all references to buttons and assign their on click commands here
		buttons [0] = GameObject.Find ("New Race Button").GetComponent<Button> ();
		buttons [1] = GameObject.Find ("Instructions Button").GetComponent<Button> ();
		buttons [2] = GameObject.Find ("Options Button").GetComponent<Button> ();
		buttons [3] = GameObject.Find ("Quit Game Button").GetComponent<Button> ();

		buttons [0].onClick.AddListener (NewRace);
		buttons [1].onClick.AddListener (Instructions);
		buttons [2].onClick.AddListener (Settings);
		buttons [3].onClick.AddListener (ExitGame);	
	}

}