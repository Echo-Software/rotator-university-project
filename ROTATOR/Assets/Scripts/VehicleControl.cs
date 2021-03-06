﻿using System.Collections;
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
	public bool invincible, offTrack, respawning, finished = false;
	public int shipGravityCharges, controllingPlayer, currentPosition, nextCheckpoint, lapCount, missileCount, finalPosition;
	public GameObject cameraHook, cameraFocus, shield;
	public BoxCollider shipCollider;

	// Private variables
	private Rigidbody ship;
	private InterfaceManager im;
	private GameManager gm;
	private PowerupManager pm;
	private Vector3 localVelocity;
	private CameraManager camera;
	[SerializeField]
	private GameObject lastCheckpoint;
	private int weaponLevel;
	private float turnAxis, turningLimit, accelerationAxis, brakingAxis, catchupSpeed, topSpeed, topAcceleration;
	private string playerInput, currentWeapon;
	private bool accelerating, braking, steering, forcedAcceleration, stunned, flipped = false;
	private bool gravityShiftReady = true;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		pm = GameObject.Find("GameManager").GetComponent<PowerupManager>();
		im = GameObject.Find ("InterfaceManager").GetComponent<InterfaceManager> ();
		ship = GetComponent<Rigidbody>();
		shipCollider = GetComponent<BoxCollider>();

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

		// Set some more variables on start
		lapCount = 1;
		nextCheckpoint = 1;
		currentPosition = controllingPlayer;
		currentWeapon = "NO WEAPON";
		weaponLevel = 0;
		topSpeed = shipTopSpeed;
		topAcceleration = shipAcceleration;
	}

    // Update is called once per frame
    void Update()
    {
		// Only allow these controls on the ship to be used if the race has started and the racer hasn't finished racing
		if (gm.raceStarted && !finished) {
			// This code is held in update instead of fixed update so that there is 0 delay on the action taking place.
			// Button to shift to other side of track. 
			if (Input.GetButtonDown(playerInput + "Y_Button") && grounded && gravityShiftReady && !respawning) {
				if (shipGravityCharges >= 1) {
					camera.CameraWhiteFlash ();
					StartCoroutine("GravityShift");                
				}
				else {
					shipCollider.isTrigger = true;
					shipCollider.isTrigger = false;
				}
			}

			// Button to respawn (self-destruct) ship
			if (Input.GetButtonDown(playerInput + "Back_Button") && !invincible && !finished) {
				RespawnShip ();
			}

			// Button to fire current weapon
			if (Input.GetButtonDown(playerInput + "A_Button") && grounded && !respawning) {
				if (currentWeapon != "NO WEAPON") {
					pm.FireWeapon (gameObject, currentWeapon, weaponLevel);
				}
			}

			// Apply proper catch up speed based on position and number of players present
			if (currentPosition == 4) {
				catchupSpeed = 1.15f;
			}
			else if (currentPosition == 3) {
				catchupSpeed = 1.10f;
			}
			else if (currentPosition == 2) {
				catchupSpeed = 1.05f;
			}
			else if (currentPosition == 1) {
				catchupSpeed = 1f;
			}
		}
    }

	void FixedUpdate() {
		// Only allow the player to control the ship once the race has started, they are grounded and not respawning or stunned
		if (grounded && !respawning && !stunned && gm.raceStarted && !finished) {
			ShipHandling ();
		}

		// If the player is no longer grounded, respawn them at the last checkpoint as they must be off the track at this point
		if (!grounded && !offTrack && !respawning && !finished) {
			StartCoroutine(TimedRespawn(lastCheckpoint));
			offTrack = true;
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

			if (localVelocity.z < shipTopSpeed * catchupSpeed){		
				ship.AddRelativeForce ((Vector3.forward * accelerationAxis) * (shipAcceleration * catchupSpeed) * 50, ForceMode.Acceleration);
			}
		} 
		else if (forcedAcceleration) {
			if (localVelocity.z < shipTopSpeed){		
				ship.AddRelativeForce ((Vector3.forward * 1) * shipAcceleration * 50, ForceMode.Acceleration);
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

	// All object collisions for vehicle handled here
    void OnTriggerEnter(Collider obj){
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
		if (obj.gameObject.tag == "Checkpoint" && !respawning) {
			if (obj.gameObject.name == "Checkpoint " + nextCheckpoint) {
				nextCheckpoint++;
				lastCheckpoint = obj.gameObject;
			} 
			else if (nextCheckpoint == 38 && obj.gameObject.name == "Finish Line") {
				gm.NewLap (controllingPlayer, lapCount);
				lapCount++;
				nextCheckpoint = 1;
				shipGravityCharges = maxGravityCharges;
				lastCheckpoint = obj.gameObject;
			}
		}

		// Collision triggers for powerup/weapon boxes
		if (obj.gameObject.tag == "Powerup") {
			currentWeapon = pm.RandomWeapon (currentPosition);
			weaponLevel = 1;
			StartCoroutine (pm.RespawnPickup(obj.gameObject));
		}

		// Collision triggers for levelup boxes
		if (obj.gameObject.tag == "Levelup") {
			StartCoroutine (pm.RespawnPickup(obj.gameObject));
			if (currentWeapon == "MISSILE" && weaponLevel == 2) {
				missileCount = 3;
			}
			if (currentWeapon != "ERASER" && currentWeapon != "NO WEAPON"){				
				if (currentWeapon == "MISSILE" && weaponLevel == 3) {
					missileCount = 3;
				} 
				else if (weaponLevel < 3) {
					weaponLevel++;
				}
			}
		}

		// Collision triggers for track speed boosters
		if (obj.gameObject.tag == "Speedup" && !forcedAcceleration) {
			StartCoroutine (SpeedUp(3f));
		}

		// Collision triggers for weapons
		if (obj.gameObject.tag == "Weapon") {
			if (!invincible && !respawning && grounded && !finished){
				// Pulse level 1 stun
				if (obj.gameObject.name == "Pulse LV1 Prefab(Clone)") {
					StartCoroutine ("Stun");
				} 
				// Level 1 and 2 mines are handled differently as they must be destroyed after impact
				else if (obj.gameObject.name == "Mine LV1 Prefab(Clone)" || obj.gameObject.name == "Mine LV2 Prefab(Clone)") {
					StartCoroutine ("Stun");
					Destroy (obj.gameObject);
				}
				// Shield level 2 has a pushback effect, but shouldn't stun or destroy anything
				else if (obj.gameObject.name == "Shield LV2 Prefab(Clone)") {
					Vector3 oppositeForce = (this.transform.position - obj.transform.position).normalized;
					ship.AddForce (oppositeForce * 17.5f, ForceMode.VelocityChange);
				}
				// All other weapon collisions result in a ship explosion
				else {				
					// replace with some sort of "destroy ship" method eventually
					RespawnShip ();

					// After the ship is destroyed, also check if it was a level 3 mine and destroy it
					if (obj.gameObject.name == "Mine LV3 Prefab(Clone)") {
						Destroy (obj.gameObject);
					}
				}
			}
		} 
		else if (obj.gameObject.tag == "Eraser") {
			// replace with some sort of "destroy ship" method eventually
			RespawnShip ();
		}
    }

	IEnumerator GravityShift(){
		// Dertermine if the ship has "flipped" or not to help with checkpoint respawning later
		if (flipped) {
			flipped = false;
		} 
		else {
			flipped = true;
		}

		gravityShiftReady = false;
		yield return new WaitForSeconds (0.25f);
		shipCollider.isTrigger = true;
		transform.Translate(new Vector3(0, -6, 0), Space.Self);
		transform.Rotate(0, 0, 180);

		// Special passives on gravity flip depending on the ship
		if (this.gameObject.name == "Echo(Clone)") {
			pm.FireWeapon (gameObject, "PULSE", 0);
		}
		else if (this.gameObject.name == "Titan(Clone)") {
			pm.FireWeapon (gameObject, "SHIELD", 0);
		}
		else if (this.gameObject.name == "Sonic(Clone)" && !forcedAcceleration) {
			StartCoroutine (SpeedUp(1.5f));
		}
		else if (this.gameObject.name == "Tundra(Clone)") {
			currentWeapon = pm.RandomWeapon (currentPosition);
			weaponLevel = 1;
		}

		shipCollider.isTrigger = false;
		shipGravityCharges -= 1;
		yield return new WaitForSeconds (gravityShiftCooldown);
		gravityShiftReady = true;
	}

	public string Speed(){
		if (!stunned) {
			if (localVelocity.z < 0.01) {
				return "0";
			} 
			if (localVelocity.z > (shipTopSpeed * catchupSpeed) - 1) {
				return Mathf.Round ((shipTopSpeed * catchupSpeed) * 10).ToString ();
			}
			else {
				return Mathf.Round (localVelocity.z * 10).ToString ();			
			}
		} 
		else {
			return "XXX";
		}

	}

	// Method that interface manager uses to get the current weapon
	public string ReturnWeapon(){
		if (currentWeapon == "NO WEAPON" || currentWeapon == "ERASER") {
			return currentWeapon;
		} 
		else if (currentWeapon == "MISSILE" && weaponLevel == 3){
			return currentWeapon + " LV." + weaponLevel.ToString() + "\n" + missileCount.ToString() + " MISSILES LEFT";
		}
		else {
			return currentWeapon + " LV." + weaponLevel.ToString();
		}
	}

	public void ResetWeapon(){
		currentWeapon = "NO WEAPON";
		weaponLevel = 1;
		missileCount = 3;
	}

	public string ReturnLap(){
		if (lapCount < 4) {
			return lapCount.ToString () + "/3";			
		} 
		else {
			if (!finished) {
				finalPosition = currentPosition;
				forcedAcceleration = true;
				StartCoroutine ("Finished");
				return "FIN";
			}
			return "FIN";
		}
	}

	public string ReturnPosition(){
		if (currentPosition == 1) {
			return currentPosition + "ST";
		} 
		else if (currentPosition == 2) {
			return currentPosition + "ND";
		} 
		else if (currentPosition == 3) {
			return currentPosition + "RD";
		} 
		else if (currentPosition == 4) {
			return currentPosition + "TH";
		} 
		else {
			return "";
		}
	}

	public void RespawnShip(){
		if (lastCheckpoint.name == "Camera Hook") {
			Debug.Log ("No passed checkpoint to respawn at");
		} 
		else if (gm.raceStarted && !finished) {
			// Stop the players velocity after they start the respawning process
			ship.angularVelocity = Vector3.zero;
			ship.velocity = Vector3.zero;
			StartCoroutine (TimedRespawn (lastCheckpoint));
		}
		else if (finished) {
			StartCoroutine (FinalRespawn (lastCheckpoint));
		}
	}

	IEnumerator TimedRespawn(GameObject respawn){
		// Sets the player invicible and respawning states to true 
		invincible = true;
		respawning = true;

		if (respawning && gm.raceStarted) {
			// Wait for 2 seconds and then reset the player position & rotation to the last checkpoint 
			// they passed, making sure to translate them 3 units away from the track so they don't clip
			yield return new WaitForSeconds (2f);
			camera.CameraWhiteFlash ();

			yield return new WaitForSeconds (0.25f);
			gameObject.transform.position = new Vector3 (respawn.transform.position.x, respawn.transform.position.y, respawn.transform.position.z);


			// Match the rotation of the checkpoint, account for the ship's flipped/unflipped state
			if (flipped) {
				gameObject.transform.eulerAngles = new Vector3 (respawn.transform.eulerAngles.x, respawn.transform.eulerAngles.y, respawn.transform.eulerAngles.z);
				transform.Translate (new Vector3 (-((2.5f - controllingPlayer) * 2.5f), -3, 0), Space.Self);
				transform.Rotate (0, 0, 180);
			} 
			else {
				gameObject.transform.eulerAngles = new Vector3 (respawn.transform.eulerAngles.x, respawn.transform.eulerAngles.y, respawn.transform.eulerAngles.z);
				transform.Translate (new Vector3 (-((2.5f - controllingPlayer) * 2.5f), 3, 0), Space.Self);
			}

			// Set the velocity of the ship to zero again, to make sure the ship doesn't drift after respawning
			ship.angularVelocity = Vector3.zero;
			ship.velocity = Vector3.zero;	

			// Reset the camera position back to the hook position instantly so the camera doesn't have to travel back to position
			camera.ResetCameraPosition ();
			DeactivateShield ();
		}
		respawning = false;

		// 2 seconds of invincibility so the player can't get repeatedly blown up etc
		yield return new WaitForSeconds (2f);
		invincible = false;
	}

	IEnumerator FinalRespawn(GameObject respawn){
		// Sets the player invicible and respawning states to true 
		invincible = true;
		respawning = true;

		if (respawning && gm.raceStarted) {
			yield return new WaitForSeconds (2f);

			gameObject.transform.position = new Vector3 (respawn.transform.position.x, respawn.transform.position.y, respawn.transform.position.z);
			gameObject.transform.eulerAngles = new Vector3 (respawn.transform.eulerAngles.x, respawn.transform.eulerAngles.y, respawn.transform.eulerAngles.z);
			transform.Translate (new Vector3 (1, 4, -1), Space.Self);


			// Set the velocity of the ship to zero again, to make sure the ship doesn't drift after respawning
			ship.angularVelocity = Vector3.zero;
			ship.velocity = Vector3.zero;	

			// Reset the camera position back to the hook position instantly so the camera doesn't have to travel back to position
			camera.ResetCameraPosition ();
			DeactivateShield ();
		}
		respawning = false;
	}

	IEnumerator Stun(){	
		stunned = true;
		invincible = true;
		ship.angularVelocity = Vector3.zero;
		ship.velocity = Vector3.zero;
		yield return new WaitForSeconds (1.5f);
		stunned = false;
		invincible = false;
	}

	IEnumerator SpeedUp(float time){
		forcedAcceleration = true;
		shipTopSpeed = topSpeed * 1.5f;
		shipAcceleration = topAcceleration * 2f;
		yield return new WaitForSeconds(time);
		forcedAcceleration = false;
		shipTopSpeed = topSpeed;
		shipAcceleration = topAcceleration;
	}

	IEnumerator Finished(){
		yield return new WaitForSeconds (0.5f);
		finished = true;
		nextCheckpoint = 4;
		lastCheckpoint = gm.podium [finalPosition - 1];
		RespawnShip ();
		forcedAcceleration = false;
		ship.isKinematic = true;
		camera.AssignCameraHooks (gm.podium[finalPosition - 1].transform.GetChild(0).gameObject, gm.podium[finalPosition - 1].transform.GetChild(1).gameObject);
		im.DeactivateUI (controllingPlayer, finalPosition);
		gm.RaceCheck ();			
	}

	public IEnumerator ActivateShield(float length){
		invincible = true;
		shield.SetActive (true);
		shield.transform.rotation = Quaternion.identity;
		yield return new WaitForSeconds (length);
		invincible = false;		
		shield.SetActive (false);
	}

	private void DeactivateShield(){
		invincible = false;
		shield.SetActive (false);
	}

	public void ResetShip(){
		respawning = false;
		invincible = false;
	}

}
