using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGravity : MonoBehaviour {

	// Public variables

	// Private variables
	private Rigidbody ship;
	private BoxCollider shipCollider;
	private VehicleControl vc;
	private Ray ray;
	private RaycastHit hit;

	// Use this for initialization
	void Start () {
		// Get the rigidbody, box collider & vehicle control components for the attached ship object
		ship = GetComponent<Rigidbody>();
		shipCollider = GetComponent<BoxCollider>();
		vc = GetComponent<VehicleControl> ();
	}

	void FixedUpdate() {
		// All actions are based on a physics raycast
		ray = new Ray (transform.position, -transform.up);
		if (Physics.Raycast (ray, out hit)) {
			GravityCheck (hit);
		} 
		else {
			// Here we can call for code to destroy/respawn ship as the raycast has missed an object (ship is OOB at this point),
			// we will also need to reset the ship "grounded" status on respawn (vc.grounded = true).
			print ("Raycast missed");
			// Tell the vehicle control script that the ship no longer hitting a track object
			vc.grounded = false;
		}
	}

	// Gravity emulation that checks if raycast hit on a track object
	void GravityCheck(RaycastHit hit){
		if (hit.transform.tag == "Track") {
			// If the vehicle is raycasting to a track piece, it is considered grounded
			vc.grounded = true;

			// Keep the rotation of the ship matching the rotation of the track normal face
			Quaternion targetRotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, Time.deltaTime * 200);

			// Keep the ship clinging to the track based on the hit distance of the raycast
			if (hit.distance > 2.5 && hit.distance < 2.7) {
				ship.AddForce (-transform.up * (Physics.gravity.magnitude / 10), ForceMode.Acceleration);
			} else if (hit.distance > 2.7 && hit.distance < 3.5) {
				ship.AddForce (-transform.up * (Physics.gravity.magnitude * 2), ForceMode.Acceleration);
			} else if (hit.distance < 2.5 && hit.distance > 2.3) {
				ship.AddForce (transform.up * (Physics.gravity.magnitude / 10), ForceMode.Acceleration);
			} else if (hit.distance < 2.3 && hit.distance > 1.5) {
				ship.AddForce (transform.up * (Physics.gravity.magnitude * 2), ForceMode.Acceleration);
			} else if (hit.distance > 3.5) {
				ship.AddForce (-transform.up * (Physics.gravity.magnitude * 20), ForceMode.Acceleration);
			} else if (hit.distance < 1.7) {
				ship.AddForce (transform.up * (Physics.gravity.magnitude * 20), ForceMode.Acceleration);
			}
		} 
	}

	// Collision detection with track object. Currently being used for debug purposes, but could be used for other things
	void OnCollisionEnter (Collision col){
		if (col.gameObject.tag == "Track") {
			Debug.Log ("Collision made with track! Something is wrong? Track incline too steep/gravity not working as intended? Or you just hit a wall, you doofus");
		}
	}

}