using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfaceManager : MonoBehaviour {

	// Public Variables
	public Camera[] cameras;
	public Image logo;

	[Header("Player 1 UI Elements (2 split)")]
	public GameObject[] p1Interface;

	[Header("Player 2 UI Elements (2 split)")]
	public GameObject[] p2Interface;

	[Header("Player 1 UI Elements (3+ split)")]
	public GameObject[] p1Interface3Plus;

	[Header("Player 2 UI Elements (3+ split)")]
	public GameObject[] p2Interface3Plus;

	[Header("Player 3 UI Elements")]
	public GameObject[] p3Interface;

	[Header("Player 4 UI Elements")]
	public GameObject[] p4Interface;

	// Private Variables
	private GameManager gm;
	private int playerNumCheck;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		playerNumCheck = gm.numberOfPlayers;
		CameraPositioning ();
		InterfaceInitialization ();
	}

	void Update(){
		InterfaceUpdate ();
	}

	void CameraPositioning(){
		// Set the cameras that are active and the rects for those cameras based on the number of players selected
		if (playerNumCheck == 1) {
			// Loop that sets all unneeded cameras to inactive
			for (int count = 1; count < 4; count++) {
				cameras [count].gameObject.SetActive (false);
			}

			// Set the required cameras rects to match the number of players
			cameras[0].rect = new Rect(new Vector2(0,0), new Vector2(1,1));
		}
		else if (playerNumCheck == 2){
			// Loop that sets all unneeded cameras to inactive
			for (int count = 2; count < 4; count++) {
				cameras [count].gameObject.SetActive (false);
			}

			// Set the required cameras rects to match the number of players
			cameras[0].rect = new Rect(new Vector2(0,0), new Vector2(0.499f,1));
			cameras[1].rect = new Rect(new Vector2(0.501f,0), new Vector2(0.499f,1));
		}
		else if (playerNumCheck == 3){
			// Set player 4 camera to inactive
			cameras [3].gameObject.SetActive (false);

			// Set the required cameras rects to match the number of players
			cameras[0].rect = new Rect(new Vector2(0,0.502f), new Vector2(0.499f,0.498f));
			cameras[1].rect = new Rect(new Vector2(0.501f,0.502f), new Vector2(0.499f,0.498f));
			cameras[2].rect = new Rect(new Vector2(0,0), new Vector2(0.499f,0.498f));

			// Set the logo to true to fill in the empty space for player 4
			logo.gameObject.SetActive (true);
		}
		else if (playerNumCheck == 4){
			// Set the required cameras rects to match the number of players
			cameras[0].rect = new Rect(new Vector2(0,0.502f), new Vector2(0.499f,0.498f));
			cameras[1].rect = new Rect(new Vector2(0.501f,0.502f), new Vector2(0.499f,0.498f));
			cameras[2].rect = new Rect(new Vector2(0,0), new Vector2(0.499f,0.498f));
			cameras[3].rect = new Rect(new Vector2(0.501f,0), new Vector2(0.499f,0.498f));
		}
	}

	void InterfaceInitialization(){
		if (playerNumCheck == 2) {

		} 
		else if (playerNumCheck == 3) {

		} 
		else if (playerNumCheck == 4) {
			p1Interface3Plus [0].SetActive (true);	
		}
	}

	void InterfaceUpdate(){
		if (playerNumCheck == 2) {

		} 
		else if (playerNumCheck == 3) {

		} 
		else if (playerNumCheck == 4) {
			// Player 1 UI updates
			float p1lap = gm.ReturnTime (0, "Lap");
			float p1total = gm.ReturnTime (0, "Total");

			p1Interface3Plus [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().Speed ();
			p1Interface3Plus [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p1lap);
			p1Interface3Plus [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p1total);
			p1Interface3Plus [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().shipGravityCharges.ToString();
		}
	}

	private string FormatTime(float timer){
		int minutes = Mathf.FloorToInt(timer / 60);
		int seconds = Mathf.FloorToInt(timer % 60);
		int milliseconds = Mathf.FloorToInt ((timer * 100) % 100);

		return minutes.ToString("00") + ":" + seconds.ToString("00") + "." + milliseconds.ToString("00");
	}
}
