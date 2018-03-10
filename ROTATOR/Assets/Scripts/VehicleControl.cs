using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControl : MonoBehaviour {

	// Public variables
	public bool grounded;
	public bool steering;

	// Private variables
	private Rigidbody ship;
	private BoxCollider shipCollider;
	private GameManager gm;
	private Vector3 localVelocity;
	private int controllingPlayer;
	private float turnAxis;
	private string playerInput;
	[SerializeField]
	private bool accelerating, braking = false;

	// Ship specific stats
	private float shipTopSpeed = 30f;
	private float shipAcceleration = 1f;
	private float shipHandlingRate = 3f;
	private int shipGravityCharges = 3;

	// Use this for initialization
	void Start () {
		// Get the rigidbody & box collider for the attached object
		ship = GetComponent<Rigidbody>();
		shipCollider = GetComponent<BoxCollider>();

		// Default player control to player1 for testing purposes. Eventually this will need to be assigned by game manager
		controllingPlayer = 1;

		// Determine which player numberered ship this script is attached to so controls can be properly assigned
		if (controllingPlayer == 1) {
			playerInput = "Player1_";
		}
		if (controllingPlayer == 2) {
			playerInput = "Player2_";
		}
		if (controllingPlayer == 3) {
			playerInput = "Player3_";
		}
		if (controllingPlayer == 4) {
			playerInput = "Player4_";
		}
	}	

	// Update is called once per frame
	void Update(){	
		// This code is held in update instead of fixed update so that there is 0 delay on the action taking place.
		// Button to shift to other side of track. 
		if (Input.GetButtonDown(playerInput + "Y_Button") && grounded){
			shipCollider.isTrigger = true;
			transform.Translate (new Vector3(0,-6,0), Space.Self);
			transform.Rotate (0, 0, 180);
			shipCollider.isTrigger = false;
		}

	}

	void FixedUpdate() {
		// Only allow the player to control the ship if they are grounded (track is underneath them)
		if (grounded) {
			ShipHandling ();
		}

	}

	// Method that handles all of the ships handling
	void ShipHandling(){
		// Axis to turn the ship
		turnAxis = Input.GetAxis(playerInput + "Horizontal");

		// Determine the local velocity of the ship for use with other elements
		localVelocity = transform.InverseTransformDirection(ship.velocity);

		// Ship handling
		if (turnAxis > 0 && (accelerating || braking)) {
			steering = true;
			ship.AddRelativeTorque (Vector3.up * turnAxis * shipHandlingRate);
		} 
		else if (turnAxis < 0 && (accelerating || braking)) {
			steering = true;
			ship.AddRelativeTorque (Vector3.up * turnAxis * shipHandlingRate);
		} 
		else if (turnAxis == 0) {
			steering = false;
			ship.angularVelocity = Vector3.zero;
		}

		// Ship accelerating
		if (Input.GetAxisRaw(playerInput + "RT_Button") != 0) {
			accelerating = true;			

			if (localVelocity.z < shipTopSpeed){				
				ship.velocity += (transform.forward * Input.GetAxisRaw(playerInput + "RT_Button")) * shipAcceleration;
			}
		} 
		else if (Input.GetAxisRaw(playerInput + "RT_Button") == 0)
		{
			accelerating = false;
		}

		// Ship braking
		if (Input.GetAxisRaw(playerInput + "LT_Button") !=0) {
			braking = true;

			if (localVelocity.z > 0) {	
				ship.velocity -= (transform.forward * Input.GetAxisRaw(playerInput + "LT_Button")) * 0.5f;
			} 
		}
		else if(Input.GetAxisRaw(playerInput + "LT_Button") == 0){
			braking = false;
		}
	}

}
