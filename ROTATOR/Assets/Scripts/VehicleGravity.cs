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
    private float shipTurn = 0.0f;
    private float turnStrength = 5f;
    private bool isAccelerating = false;
    private bool isBraking = false;

	// Use this for initialization
	void Start () {
		// Get the rigidbody & box collider for the attached object
		ship = GetComponent<Rigidbody>();
		shipCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        // Axis to turn the ship
        float turnAxis = Input.GetAxis("Horizontal");
        shipTurn = turnAxis;
    }

	void FixedUpdate() {
		// All action are based on a physics raycast
		ray = new Ray (transform.position, -transform.up);
		if (Physics.Raycast (ray, out hit)) {
			TestDriving ();
			GravityCheck (hit);
		} 
		else {
			// Call for code to destroy/respawn ship as the raycast has missed a track object (ship is OOB at this point)
			print ("Raycast missed");
		}
	}

	// Gravity emulation that checks if raycast hit on a track object
	void GravityCheck(RaycastHit hit){
		// Debug logged hit distance for testing purposes (can be deleted when no longer needed)
		Debug.Log (hit.distance);

		if (hit.transform.tag == "Track") {
			// Keep the rotation of the ship matching the rotation of the track normal face
			Quaternion targetRotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, Time.deltaTime * 200);

			// Keep the ship clinging to the track based on the hit distance of the raycast
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
	}

	// All code is used to test the ship steering/acceleration etc.. Code should be moved to new script in the future
	void TestDriving(){

        // Turning the ship
        if (shipTurn > 0.15){
			ship.AddRelativeTorque(Vector3.up * shipTurn * turnStrength);
		}
		else if (shipTurn < 0.15){
			ship.AddRelativeTorque(Vector3.up * shipTurn * turnStrength);
		}
		// Velocity button
		if (Input.GetAxisRaw("RT_Button") != 0) {
                ship.AddRelativeForce(Vector3.forward / 1.5f, ForceMode.VelocityChange);
                isAccelerating = true;
		} 
        else if (Input.GetAxisRaw("RT_Button") == 0 )
        {
            isAccelerating = false;
        }
		// Velocity stop button
		if (Input.GetAxisRaw("LT_Button") !=0) {
			ship.velocity = Vector3.zero;
            isBraking = true;
		}
        else if(Input.GetAxisRaw("LT_Button") == 0)
        {
            isBraking = false;
        }
		// Button to shift to other side of track
		if (Input.GetButtonDown("Y_Button") && hit.transform.tag == "Track"){
			shipCollider.isTrigger = true;
			transform.Translate (new Vector3(0,-6,0), Space.Self);
			transform.Rotate (0, 0, 180);
			shipCollider.isTrigger = false;
		}
	}

}