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
	[Range(2,4)]
	public float gravityShiftCooldown;
	public int shipGravityCharges, controllingPlayer;
	public GameObject cameraHook, cameraFocus;


	// Private variables
	private Rigidbody ship;
	private BoxCollider shipCollider;
	private GameManager gm;
	private Vector3 localVelocity;
	private CameraManager camera;
	private float turnAxis, turningLimit, accelerationAxis, brakingAxis;
	private string playerInput;
	private bool accelerating, braking, steering = false;
	private bool gravityShiftReady = true;
	private int nextCheckpoint, lapCount;

	// Use this for initialization
	void Start () {
		// Get the rigidbody & box collider for the attached object, also assign the game manager
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		ship = GetComponent<Rigidbody>();
		shipCollider = GetComponent<BoxCollider>();

		// Default player control to player1 for testing purposes. Eventually this will need to be assigned by game manager
		// controllingPlayer = 1;

		// Determine which player numberered ship this script is attached to so controls can be properly assigned
		if (controllingPlayer == 1) {
			playerInput = "Player1_";
			camera = GameObject.Find("Camera 1").GetComponent<CameraManager>();
			camera.AssignCameraHooks (cameraHook, cameraFocus);
		}
		if (controllingPlayer == 2) {
			playerInput = "Player2_";
			camera = GameObject.Find("Camera 2").GetComponent<CameraManager>();
			camera.AssignCameraHooks (cameraHook, cameraFocus);
		}
		if (controllingPlayer == 3) {
			playerInput = "Player3_";
			camera = GameObject.Find("Camera 3").GetComponent<CameraManager>();
			camera.AssignCameraHooks (cameraHook, cameraFocus);
		}
		if (controllingPlayer == 4) {
			playerInput = "Player4_";
			camera = GameObject.Find("Camera 4").GetComponent<CameraManager>();
			camera.AssignCameraHooks (cameraHook, cameraFocus);
		}

		// Set the ship moving an indistinguishable amount velocity gives an accurate reading
		ship.velocity = transform.forward * -0.001f;

		// Set the ships initial gravity charges based on their max amount
		shipGravityCharges = maxGravityCharges;

		// Set the initial lap and checkpoint counters
		lapCount = 1;
		nextCheckpoint = 1;
	}

    // Update is called once per frame
    void Update()
    {
        // This code is held in update instead of fixed update so that there is 0 delay on the action taking place.
        // Button to shift to other side of track. 
        if (Input.GetButtonDown(playerInput + "Y_Button") && grounded && gravityShiftReady)
        {
            if (shipGravityCharges >= 1)
            {
				camera.CameraWhiteFlash ();
				StartCoroutine("GravityShift");                
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
		// Collision triggers for gravity charge item
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

		// Collision triggers for lap checkpoint
		if (obj.gameObject.tag == "Checkpoint") {
			if (obj.gameObject.name == "Checkpoint " + nextCheckpoint) {
				nextCheckpoint++;
			} 
			else if (nextCheckpoint > gm.checkpoints.Length - 1 && obj.gameObject.name == "Finish Line") {
				gm.NewLap (controllingPlayer, lapCount);
				lapCount++;
				nextCheckpoint = 1;

			}
		}
    }

	IEnumerator GravityShift(){
		gravityShiftReady = false;
		yield return new WaitForSeconds (0.25f);
		shipCollider.isTrigger = true;
		transform.Translate(new Vector3(0, -6, 0), Space.Self);
		transform.Rotate(0, 0, 180);
		shipCollider.isTrigger = false;
		shipGravityCharges -= 1;
		yield return new WaitForSeconds (gravityShiftCooldown);
		gravityShiftReady = true;
	}

	public string Speed(){
		if (localVelocity.z < 0.01) {
			return "0";
		} 
		if (localVelocity.z > shipTopSpeed - 1) {
			return Mathf.Round(shipTopSpeed * 10).ToString();
		}
		else {
			return Mathf.Round(localVelocity.z * 10).ToString();			
		}
	}

	public string LapCount(){
		if (lapCount < 4) {
			return lapCount.ToString () + "/3";			
		} 
		else {
			return "FIN";
		}
	}

}
