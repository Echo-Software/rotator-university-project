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
	private float turningLimit;
	private float accelerationAxis;
	private float brakingAxis;
	private string playerInput;
	[SerializeField]
	private bool accelerating, braking = false;

	// Ship specific stats
	private float shipTopSpeed = 30f;
	private float shipAcceleration = 1f;
	private float shipHandlingRate = 30f;
	private int shipGravityCharges = 3;
    private int maxGravityCharges = 3;

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

		// Set the ship moving an indistinguishable amount velocity gives an accurate reading
		ship.velocity = transform.forward * -0.001f;
	}

    // Update is called once per frame
    void Update()
    {
        // This code is held in update instead of fixed update so that there is 0 delay on the action taking place.
        // Button to shift to other side of track. 
        if (Input.GetButtonDown(playerInput + "Y_Button") && grounded)
        {
            if (shipGravityCharges >= 1)
            {
                shipCollider.isTrigger = true;
                transform.Translate(new Vector3(0, -6, 0), Space.Self);
                transform.Rotate(0, 0, 180);
                shipCollider.isTrigger = false;
                shipGravityCharges -= 1;
            }
            else
            {
                shipCollider.isTrigger = true;
                shipCollider.isTrigger = false;
            }
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
		// Store the turning, acceleration and braking axis values in variables
		turnAxis = Input.GetAxis(playerInput + "Horizontal");
		accelerationAxis = Input.GetAxisRaw (playerInput + "RT_Button");
		brakingAxis = Input.GetAxisRaw (playerInput + "LT_Button");

		// Determine the local velocity of the ship for use with other elements
		localVelocity = transform.InverseTransformDirection (ship.velocity);

		// Set a variable that limits how much the user can turn the ship based on how fast they are currently going
		turningLimit = localVelocity.z / 10;

		// Make sure the turning limit never goes below 5
		if (turningLimit < 5) {
			turningLimit = 5;
		}

		// Ship turning controls
		if (turnAxis > 0 && (localVelocity.z > 0 || braking)) {
			steering = true;
			ship.AddRelativeTorque (Vector3.up * turnAxis * (shipHandlingRate / turningLimit));
		} 
		else if (turnAxis < 0 && (localVelocity.z > 0 || braking)) {
			steering = true;
			ship.AddRelativeTorque (Vector3.up * turnAxis * (shipHandlingRate / turningLimit));
		} 
		else if (turnAxis == 0) {
			steering = false;
			ship.angularVelocity = Vector3.zero;
		}

		// Ship accelerating
		if (accelerationAxis != 0) {
			accelerating = true;			

			if (localVelocity.z < shipTopSpeed){				
				ship.velocity += (transform.forward * accelerationAxis) * shipAcceleration;
			}
		} 
		else {
			accelerating = false;
		}

		// Ship braking
		if (brakingAxis !=0) {
			braking = true;

			if (localVelocity.z > 0) {	
				ship.velocity -= (transform.forward * brakingAxis) * 0.5f;
			} 
		}
		else {
			braking = false;
		}
	}

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "GravCharge")
        {
            if (shipGravityCharges < maxGravityCharges)
            { 
                shipGravityCharges += 1;
            }
            else
            {
                shipGravityCharges = maxGravityCharges;
            }
        }
    }

}
