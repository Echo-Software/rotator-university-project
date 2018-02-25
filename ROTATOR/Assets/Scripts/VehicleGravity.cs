using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGravity : MonoBehaviour {

	// Public variables

	// Private variables
	private Rigidbody ship;
	private BoxCollider shipCollider;
	private Ray ray;
	private RaycastHit hit;

	// Use this for initialization
	void Start () {
		// Get the rigidbody & box collider for the attached object
		ship = GetComponent<Rigidbody>();
		shipCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		ray = new Ray (transform.position, -transform.up);

		// Velocity button (remove when done using for testing)
		if (Input.GetKey("space")) {
			ship.AddRelativeForce (Vector3.forward / 1.5f, ForceMode.VelocityChange);
		} 
		// Velocity stop button (remove when doing using for testing)
		if (Input.GetKey("x")) {
			ship.velocity = Vector3.zero;
		}
		// Button to shift to other side of track (code a better version after testing)
		if (Input.GetKeyDown("z") && hit.transform.tag == "Track"){
			shipCollider.isTrigger = true;
			transform.Translate (new Vector3(0,-6,0), Space.Self);
			transform.Rotate (0, 0, 180);
			shipCollider.isTrigger = false;
		}

		/* (Old) Gravity - world emulation
		if (gravityReversed) {
			ship.AddForce (Vector3.up * Physics.gravity.magnitude);
		} 
		else {
			ship.AddForce (-Vector3.up * Physics.gravity.magnitude);
		}
		*/

		// Gravity (based on raycast hit on track object)
		if (Physics.Raycast (ray, out hit)) {
			Debug.Log (hit.distance);

			if (hit.transform.tag == "Track") {
				Quaternion targetRotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
				transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, Time.deltaTime * 200);

				if (hit.distance > 2.5 && hit.distance < 2.7) {
					ship.AddForce (-transform.up * (Physics.gravity.magnitude / 10));
				} else if (hit.distance > 2.7 && hit.distance < 3.5) {
					ship.AddForce (-transform.up * (Physics.gravity.magnitude * 2), ForceMode.Acceleration);
				} else if (hit.distance < 2.5 && hit.distance > 2.3) {
					ship.AddForce (transform.up * (Physics.gravity.magnitude / 10));
				} else if (hit.distance < 2.3 && hit.distance > 1.5) {
					// ship.velocity = Vector3.zero;
					ship.AddForce (transform.up * (Physics.gravity.magnitude * 2), ForceMode.Acceleration);
				} else if (hit.distance > 3.5) {
					ship.AddForce (-transform.up * (Physics.gravity.magnitude * 20), ForceMode.Acceleration);
				} else if (hit.distance < 1.5) {
					ship.AddForce (transform.up * (Physics.gravity.magnitude * 20), ForceMode.Acceleration);
				}
			}							
			else {
				print ("Not a track object");
			}
		}
	}

}