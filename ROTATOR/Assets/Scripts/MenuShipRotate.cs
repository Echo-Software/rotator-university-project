using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShipRotate : MonoBehaviour {

	public GameObject[] menuShips = new GameObject[4];

	void Start () {
		menuShips [Random.Range (0, 4)].SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(-Vector3.forward * Time.deltaTime * 50);
	}

}