using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuShipRotate : MonoBehaviour {

	public GameObject[] menuShips = new GameObject[4];

	void Start () {
		menuShips [Random.Range (0, 4)].SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(-Vector3.forward * Time.deltaTime * 50);
	}

	public void NewGame() {
		// Point this to ship selection once it's been added.. for now just loads the "Track" scene and starts a race with default ship selection/player number
		SceneManager.LoadScene("Track", LoadSceneMode.Single);
	}

	public void Instructions() {
		// Point this to instructions on how to play once added
		SceneManager.LoadScene("Track", LoadSceneMode.Single);
	}

	public void Settings() {
		// Point this to settings once added
		SceneManager.LoadScene("Track", LoadSceneMode.Single);
	}

	public void ExitGame(){
		// Replace this with loading the main menu scene (once created)
		Application.Quit ();
	}

}