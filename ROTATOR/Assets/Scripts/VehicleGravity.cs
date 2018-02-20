using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGravity : MonoBehaviour {

	// Public variables

	// Private variables
	Rigidbody ship;
	RaycastHit hit;
	Vector3 reflect;

	// Use this for initialization
	void Start () {
		// Get the rigidbody for the attached object
		ship = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine (transform.position, hit.point, Color.red);
	}

	void FixedUpdate() {

		/* Gravity (world emulation)
		if (gravityReversed) {
			ship.AddForce (Vector3.up * Physics.gravity.magnitude);
		} 
		else {
			ship.AddForce (-Vector3.up * Physics.gravity.magnitude);
		}
		*/

		// Gravity (based on raycast hit on track object)
		if (Physics.Raycast (transform.position, -transform.up, out hit)) {
			Debug.Log (hit.distance);
			if (hit.transform.tag == "Track") {
				if (hit.distance > 1.5) {
					ship.AddForce (-transform.up * (Physics.gravity.magnitude / 10));
				} 
				else if (hit.distance < 1.5) {
					ship.AddForce (transform.up * (Physics.gravity.magnitude / 10));
				}
				transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
			} 
			else {
				print ("Not a track object");
			}
		}

	}
}
