using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour {

	// Public Variables
	public Camera[] cameras;
	public Image logo;

	// Private Variables
	private GameManager gm;
	private int playerNumCheck;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		playerNumCheck = gm.numberOfPlayers;
		CameraPositioning ();
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

}
