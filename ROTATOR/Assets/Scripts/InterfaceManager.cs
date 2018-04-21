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

	[Header("Other UI Elements")]
	public GameObject[] otherUI;

	// Private Variables
	private GameManager gm;
	private int playerNumCheck;
	private float p1lap, p2lap, p3lap, p4lap, p1total, p2total, p3total, p4total;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		playerNumCheck = gm.numberOfPlayers;
		CameraPositioning ();
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
			p1Interface [0].SetActive (true);
			p2Interface [0].SetActive (true);	
		} 
		else if (playerNumCheck == 3) {
			p1Interface3Plus [0].SetActive (true);
			p2Interface3Plus [0].SetActive (true);	
			p3Interface [0].SetActive (true);
		} 
		else if (playerNumCheck == 4) {
			p1Interface3Plus [0].SetActive (true);
			p2Interface3Plus [0].SetActive (true);	
			p3Interface [0].SetActive (true);	
			p4Interface [0].SetActive (true);	
		}
	}

	void InterfaceUpdate(){
		if (gm.numberOfPlayers == 2) {
			// Player 1 UI updates
			p1lap = gm.ReturnTime (0, "Lap");
			p1total = gm.ReturnTime (0, "Total");

			p1Interface [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().Speed ();
			p1Interface [2].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().ReturnPosition ();
			p1Interface [3].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().ReturnLap ();
			p1Interface [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p1lap);
			p1Interface [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p1total);
			p1Interface [6].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().ReturnWeapon();
			p1Interface [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().shipGravityCharges.ToString();

			// Player 2 UI updates
			p2lap = gm.ReturnTime (0, "Lap");
			p2total = gm.ReturnTime (0, "Total");

			p2Interface [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().Speed ();
			p2Interface [2].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().ReturnPosition ();
			p2Interface [3].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().ReturnLap ();
			p2Interface [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p2lap);
			p2Interface [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p2total);
			p2Interface [6].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().ReturnWeapon();
			p2Interface [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().shipGravityCharges.ToString();
		} 
		else if (gm.numberOfPlayers == 3) {
			// Player 1 UI updates
			p1lap = gm.ReturnTime (0, "Lap");
			p1total = gm.ReturnTime (0, "Total");

			p1Interface3Plus [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().Speed ();
			p1Interface3Plus [2].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().ReturnPosition ();
			p1Interface3Plus [3].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().ReturnLap ();
			p1Interface3Plus [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p1lap);
			p1Interface3Plus [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p1total);
			p1Interface3Plus [6].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().ReturnWeapon();
			p1Interface3Plus [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().shipGravityCharges.ToString();

			// Player 2 UI updates
			p2lap = gm.ReturnTime (0, "Lap");
			p2total = gm.ReturnTime (0, "Total");

			p2Interface3Plus [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().Speed ();
			p2Interface3Plus [2].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().ReturnPosition ();
			p2Interface3Plus [3].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().ReturnLap ();
			p2Interface3Plus [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p2lap);
			p2Interface3Plus [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p2total);
			p2Interface3Plus [6].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().ReturnWeapon();
			p2Interface3Plus [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().shipGravityCharges.ToString();

			// Player 3 UI updates
			p3lap = gm.ReturnTime (0, "Lap");
			p3total = gm.ReturnTime (0, "Total");

			p3Interface [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl>().Speed ();
			p3Interface [2].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl>().ReturnPosition ();
			p3Interface [3].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl>().ReturnLap ();
			p3Interface [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p3lap);
			p3Interface [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p3total);
			p3Interface [6].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl>().ReturnWeapon();
			p3Interface [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl>().shipGravityCharges.ToString();

		} 
		else if (gm.numberOfPlayers == 4) {
			// Player 1 UI updates
			p1lap = gm.ReturnTime (0, "Lap");
			p1total = gm.ReturnTime (0, "Total");

			p1Interface3Plus [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().Speed ();
			p1Interface3Plus [2].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().ReturnPosition ();
			p1Interface3Plus [3].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().ReturnLap ();
			p1Interface3Plus [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p1lap);
			p1Interface3Plus [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p1total);
			p1Interface3Plus [6].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().ReturnWeapon();
			p1Interface3Plus [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [0].GetComponent<VehicleControl>().shipGravityCharges.ToString();

			// Player 2 UI updates
			p2lap = gm.ReturnTime (0, "Lap");
			p2total = gm.ReturnTime (0, "Total");

			p2Interface3Plus [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().Speed ();
			p2Interface3Plus [2].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().ReturnPosition ();
			p2Interface3Plus [3].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().ReturnLap ();
			p2Interface3Plus [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p2lap);
			p2Interface3Plus [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p2total);
			p2Interface3Plus [6].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().ReturnWeapon();
			p2Interface3Plus [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [1].GetComponent<VehicleControl>().shipGravityCharges.ToString();

			// Player 3 UI updates
			p3lap = gm.ReturnTime (0, "Lap");
			p3total = gm.ReturnTime (0, "Total");

			p3Interface [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl>().Speed ();
			p3Interface [2].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl>().ReturnPosition ();
			p3Interface [3].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl> ().ReturnLap ();
			p3Interface [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p3lap);
			p3Interface [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p3total);
			p3Interface [6].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl>().ReturnWeapon();
			p3Interface [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [2].GetComponent<VehicleControl>().shipGravityCharges.ToString();

			// Player 4 UI updates
			p4lap = gm.ReturnTime (0, "Lap");
			p4total = gm.ReturnTime (0, "Total");

			p4Interface [1].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [3].GetComponent<VehicleControl>().Speed ();
			p4Interface [2].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [3].GetComponent<VehicleControl>().ReturnPosition ();
			p4Interface [3].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [3].GetComponent<VehicleControl>().ReturnLap ();
			p4Interface [4].GetComponent<TextMeshProUGUI> ().text = FormatTime (p4lap);
			p4Interface [5].GetComponent<TextMeshProUGUI> ().text = FormatTime (p4total);
			p4Interface [6].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [3].GetComponent<VehicleControl>().ReturnWeapon();
			p4Interface [7].GetComponent<TextMeshProUGUI> ().text = gm.playerShipSelection [3].GetComponent<VehicleControl>().shipGravityCharges.ToString();
		}
	}

	private string FormatTime(float timer){
		int minutes = Mathf.FloorToInt(timer / 60);
		int seconds = Mathf.FloorToInt(timer % 60);
		int milliseconds = Mathf.FloorToInt ((timer * 100) % 100);

		return minutes.ToString("00") + ":" + seconds.ToString("00") + "." + milliseconds.ToString("00");
	}

	public IEnumerator Countdown(){
		otherUI [0].SetActive (true);
		otherUI [0].GetComponent<TextMeshProUGUI> ().text = "3";
		yield return new WaitForSeconds (1f);
		otherUI [0].GetComponent<TextMeshProUGUI> ().text = "2";
		yield return new WaitForSeconds (1f);
		otherUI [0].GetComponent<TextMeshProUGUI> ().text = "1";
		yield return new WaitForSeconds (1f);
		otherUI [0].GetComponent<TextMeshProUGUI> ().text = "GO";
		gm.raceStarted = true;
		InterfaceInitialization ();
		yield return new WaitForSeconds (0.25f);
		otherUI [0].SetActive (false);
		yield return new WaitForSeconds (0.25f);
		otherUI [0].SetActive (true);
		yield return new WaitForSeconds (0.25f);
		otherUI [0].SetActive (false);
		yield return new WaitForSeconds (0.25f);
		otherUI [0].SetActive (true);
		yield return new WaitForSeconds (0.25f);
		otherUI [0].SetActive (false);
	}

}
