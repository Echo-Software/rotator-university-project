using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControl : MonoBehaviour {

	// Public variables
	public bool grounded;

	// Ship specific stats, with min/max ranges set for making prefabs
	[Range(25,50)]
	public float shipTopSpeed;
	[Range(0.5f,1.6f)]
	public float shipAcceleration;
	[Range(15,100)]
	public float shipHandlingRate;
	[Range(2,4)]
	public int maxGravityCharges;

	// Private variables
	private Rigidbody ship;
	private BoxCollider shipCollider;
	private GameManager gm;
	private Vector3 localVelocity;
	private int controllingPlayer;
	private float turnAxis, turningLimit, accelerationAxis, brakingAxis;
	private int shipGravityCharges = 3;
	private string playerInput;
	private bool accelerating, braking, steering = false;

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
		Debug.Log (localVelocity.z);
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
		turnAxis = Input.GetAxisRaw(playerInput + "Horizontal");
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
			ship.AddRelativeTorque (Vector3.up * turnAxis * (shipHandlingRate / turningLimit), ForceMode.Acceleration);
		} 
		else if (turnAxis < 0 && (localVelocity.z > 0 || braking)) {
			steering = true;
			ship.AddRelativeTorque (Vector3.up * turnAxis * (shipHandlingRate / turningLimit), ForceMode.Acceleration);
		} 
		else if (turnAxis == 0) {
			steering = false;
			ship.angularVelocity = Vector3.zero;
		}

		// Ship accelerating
		if (accelerationAxis != 0) {
			accelerating = true;			

			if (localVelocity.z < shipTopSpeed){		
				ship.AddRelativeForce ((Vector3.forward * accelerationAxis) * shipAcceleration * 50, ForceMode.Acceleration);
			}
		} 
		else {
			accelerating = false;
		}

		// Ship braking
		if (brakingAxis !=0) {
			braking = true;

			if (localVelocity.z > 0) {	
				ship.AddRelativeForce ((-Vector3.forward * brakingAxis) * 25, ForceMode.Acceleration);
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
