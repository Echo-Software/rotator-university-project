using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGravity : MonoBehaviour {

	// Public variables
	public bool gravityReversed;

	// Private variables
	Rigidbody ship;

	// Use this for initialization
	void Start () {
		// Get the rigidbody for the attached object
		ship = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (gravityReversed) {
			ship.AddForce (Vector3.up * Physics.gravity.magnitude);
		} 
		else {
			ship.AddForce (-Vector3.up * Physics.gravity.magnitude);
		}
	}
}
