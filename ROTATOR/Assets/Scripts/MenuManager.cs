using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	// Private variables
	private AudioSource audioSource;
	private int numberOfPlayers = 4;
	private GameObject[] selectedShips;
	private static MenuManager instanceReference;
	private Button[] buttons = new Button[4];

	// Public variables
	public bool buttonCheck = false;
	public AudioClip[] music;
	public GameObject[] selectableShips = new GameObject[4];
	public Image brightnessOverlay;

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
			buttonCheck = false;
		}
	}

	public int GetPlayerNumber(){
		return numberOfPlayers;
	}

	public GameObject[] GetShipSelection(){
		return selectedShips;
	}

	public void PlayMenuLoop(){
		audioSource.Stop ();
		audioSource.clip = music [0];
		audioSource.Play ();
		audioSource.loop = true;		
	}

	public void ResetButtons(){
		// Get all references to buttons and assign their on click commands here
		buttons[0] = GameObject.Find("New Race Button").GetComponent<Button>();
		buttons[1] = GameObject.Find("Instructions Button").GetComponent<Button>();
		buttons[2] = GameObject.Find("Options Button").GetComponent<Button>();
		buttons[3] = GameObject.Find("Quit Game Button").GetComponent<Button>();

		buttons [0].onClick.AddListener (NewRace);
		buttons [1].onClick.AddListener (Instructions);
		buttons [2].onClick.AddListener (Settings);
		buttons [3].onClick.AddListener (ExitGame);
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
		// Load "NewRace" when ready
		SceneManager.LoadScene("Track", LoadSceneMode.Single);

		// Use this code when loading into the race after ship selection
		audioSource.Stop ();
		audioSource.clip = music [1];
		audioSource.Play ();
		audioSource.loop = true;
	}

	public void Instructions() {
		SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
	}

	public void Settings() {
		// Point this to settings once added
		SceneManager.LoadScene("Settings", LoadSceneMode.Single);
	}

	public void ExitGame(){
		// Replace this with loading the main menu scene (once created)
		Application.Quit ();
	}

}