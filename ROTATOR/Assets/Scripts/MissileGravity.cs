using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileGravity : MonoBehaviour {

	// Public Variables
	public GameObject target;

	// Private variables
	private Rigidbody missile;
	private Ray ray;
	private RaycastHit hit;
	private VehicleControl vc;
	private GameManager gm;

	// Use this for initialization
	void Start () {
		// Get the rigidbody, box collider & vehicle control components for the attached ship object
		missile = GetComponent<Rigidbody>();
		vc = GetComponentInParent<VehicleControl> ();
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	void FixedUpdate() {
		// All actions are based on a physics raycast
		ray = new Ray (transform.position, -transform.up);
		if (Physics.Raycast (ray, out hit)) {
			GravityCheck (hit);
		} 
		else {			
			print ("Raycast missed");
			Destroy (this);
		}

		if (gameObject.name == "Missile LV1 Prefab(Clone)") {
			missile.AddRelativeForce (Vector3.forward * 100 - missile.velocity);
		} else {		

			if (vc.currentPosition == 1) {
				for (int count = 0; count < gm.numberOfPlayers; count++) {
					if (gm.playerShipSelection [count].GetComponent<VehicleControl> ().currentPosition == 2) {
						target = gm.playerShipSelection [count];
					}
				}
			} else { 
				for (int count = 0; count < gm.numberOfPlayers; count++) {
					if (gm.playerShipSelection [count].GetComponent<VehicleControl> ().currentPosition == vc.currentPosition-1) {
						target = gm.playerShipSelection [count];
					}
				}
			}
			transform.LookAt (target.transform.position);
			missile.AddRelativeForce (0, 0, 100);
		}

	}

	// Gravity emulation that checks if raycast hit on a track object
	void GravityCheck(RaycastHit hit){
		if (hit.transform.tag == "Track") {

			// Keep the rotation of the missile matching the rotation of the track normal face
			Quaternion targetRotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, Time.deltaTime * 200);

			// Keep the missile clinging to the track based on the hit distance of the raycast
			if (hit.distance > 2.5 && hit.distance < 2.7) {
				missile.AddForce (-transform.up * (Physics.gravity.magnitude / 10), ForceMode.Acceleration);
			} else if (hit.distance > 2.7 && hit.distance < 3.5) {
				missile.AddForce (-transform.up * (Physics.gravity.magnitude * 2), ForceMode.Acceleration);
			} else if (hit.distance < 2.5 && hit.distance > 2.3) {
				missile.AddForce (transform.up * (Physics.gravity.magnitude / 10), ForceMode.Acceleration);
			} else if (hit.distance < 2.3 && hit.distance > 1.5) {
				missile.AddForce (transform.up * (Physics.gravity.magnitude * 2), ForceMode.Acceleration);
			} else if (hit.distance > 3.5) {
				missile.AddForce (-transform.up * (Physics.gravity.magnitude * 20), ForceMode.Acceleration);
			} else if (hit.distance < 1.7) {
				missile.AddForce (transform.up * (Physics.gravity.magnitude * 20), ForceMode.Acceleration);
			}
		} 
	}

	// Collision detection with track object. Currently being used for debug purposes, but could be used for other things
	void OnTriggerEnter (Collider col){
		if (col.gameObject.tag == "Track") {
			Debug.Log ("Missile hit the track and exploded");
			Destroy (gameObject);
		}
		if (col.gameObject.tag == "Player") {
			Debug.Log ("Missile hit a player and exploded");
			Destroy (gameObject);
		}
	}

}
