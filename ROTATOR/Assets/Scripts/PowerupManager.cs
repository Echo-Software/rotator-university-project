using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

	// Public Variables
	public GameObject[] weaponPrefabs;

	// Private Variables
	private GameManager gm;
	private string[] weaponTypes = new string[] {"PULSE","MISSILE","SHIELD","MINE","ERASER"};
	private GameObject target;

	void Start(){
		gm = gameObject.GetComponent<GameManager> ();
	}

	public string RandomWeapon(int currentPosition){
		// Check if the player is in last position before deciding which weapon they get (only last place can get the Eraser)
		// If the player is in last place, they have a 10% chance to get the eraser, otherwise they get one of the other 4 weapons
		if (currentPosition == gm.numberOfPlayers && Random.Range (0, 11) == 10) {
			return weaponTypes [4];
		} 
		else {
			return weaponTypes [Random.Range (0, 4)];
		}
	}

	// When called sets the powerup/levelup item box to inactive for a few seconds before reactivating it
	public IEnumerator RespawnPickup(GameObject itemBox){
		itemBox.SetActive (false);
		yield return new WaitForSeconds (2f);
		itemBox.SetActive (true);
	}

	// Script that uses a powerup depending on what weapon name and weapon level is passed through the method
	public void FireWeapon(GameObject player, string weaponName, int weaponLevel){
		GameObject tempPrefab;

		// Pulse weapon code
		if (weaponName == "PULSE") {
			if (weaponLevel == 1) {
				// Fire pulse AOE
				Debug.Log("Level 1 (regular) pulse AOE fired");
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [0], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<SphereCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				tempPrefab.GetComponent<SphereCollider> ().enabled = true;
				tempPrefab.transform.parent = player.transform;
				StartCoroutine(SpherePulseRadius(player, 0.75f));
				Destroy (tempPrefab, 2.0f);
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
			if (weaponLevel == 2) {
				// Fire explosive pulse AOE
				Debug.Log("Level 2 (explosive) pulse AOE fired");
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [1], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<SphereCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				tempPrefab.GetComponent<SphereCollider> ().enabled = true;
				tempPrefab.transform.parent = player.transform;
				StartCoroutine(SpherePulseRadius(player, 0.90f));
				Destroy (tempPrefab, 2.0f);
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
			if (weaponLevel == 3) {
				// Fire explosive pulse AOE that effects both sides of track
				Debug.Log("Level 3 (explosive) pulse AOE fired on both sides of track");
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [2], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<CapsuleCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				tempPrefab.GetComponent<CapsuleCollider> ().enabled = true;
				tempPrefab.transform.parent = player.transform;
				StartCoroutine(CapsulePulseRadius(player, 0.9f));
				Destroy (tempPrefab, 2.0f);
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
		}

		// Missile weapon code
		if (weaponName == "MISSILE") {
			if (weaponLevel == 1) {
				// Fire missile
				Debug.Log("Level 1 (regular) missile fired");
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [3], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<CapsuleCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				tempPrefab.GetComponent<CapsuleCollider> ().enabled = true;
				tempPrefab.transform.parent = player.transform;
				tempPrefab.GetComponent<Rigidbody>().AddRelativeForce (Vector3.forward * 50, ForceMode.Impulse);
				Destroy (tempPrefab, 4.0f);
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
			if (weaponLevel == 2) {
				// Fire homing missile
				Debug.Log("Level 2 (homing) missile fired");
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [4], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<CapsuleCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				tempPrefab.GetComponent<CapsuleCollider> ().enabled = true;
				tempPrefab.transform.parent = player.transform;
				Destroy (tempPrefab, 4.0f);
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
			if (weaponLevel == 3) {
				if (player.GetComponent<VehicleControl> ().missileCount > 1) {
					// Fire homing missile from cache
					player.GetComponent<VehicleControl> ().missileCount--;
					Debug.Log("Level 3 (homing) missile fired. " + player.GetComponent<VehicleControl> ().missileCount + " missiles left");
					tempPrefab = (GameObject)Instantiate (weaponPrefabs [4], player.transform.position, player.transform.rotation);
					Physics.IgnoreCollision(tempPrefab.GetComponent<CapsuleCollider>(), player.GetComponent<VehicleControl>().shipCollider);
					tempPrefab.GetComponent<CapsuleCollider> ().enabled = true;
					tempPrefab.transform.parent = player.transform;
					Destroy (tempPrefab, 4.0f);
				} 
				else {
					// Fire final homing missile from cache
					Debug.Log("Level 3 (homing) missile fired. 0 missiles left");
					tempPrefab = (GameObject)Instantiate (weaponPrefabs [4], player.transform.position, player.transform.rotation);
					Physics.IgnoreCollision(tempPrefab.GetComponent<CapsuleCollider>(), player.GetComponent<VehicleControl>().shipCollider);
					tempPrefab.GetComponent<CapsuleCollider> ().enabled = true;
					tempPrefab.transform.parent = player.transform;
					Destroy (tempPrefab, 4.0f);
					player.GetComponent<VehicleControl> ().ResetWeapon ();
				}
			}
		}

		// Shield weapon code
		if (weaponName == "SHIELD") {
			if (weaponLevel == 1) {
				// Activate protective shield
				Debug.Log("Level 1 (regular) shield activated");
				StartCoroutine(player.GetComponent<VehicleControl> ().ActivateShield (2f));
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
			if (weaponLevel == 2) {
				// Activate protective shield with added knockback effect
				Debug.Log("Level 2 (knockback) shield activated");
				StartCoroutine(player.GetComponent<VehicleControl> ().ActivateShield (3f));
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [5], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<SphereCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				tempPrefab.GetComponent<SphereCollider> ().enabled = true;
				tempPrefab.transform.parent = player.transform;
				StartCoroutine(SpherePulseRadius(player, 0.75f));
				Destroy (tempPrefab, 3.0f);
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
			if (weaponLevel == 3) {
				// Activate explosive protective shield
				Debug.Log("Level 3 (explosive) shield activated");
				StartCoroutine(player.GetComponent<VehicleControl> ().ActivateShield (3f));
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [6], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<SphereCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				tempPrefab.GetComponent<SphereCollider> ().enabled = true;
				tempPrefab.transform.parent = player.transform;
				StartCoroutine(SpherePulseRadius(player, 0.5f));
				Destroy (tempPrefab, 3.0f);
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
		} 

		// Mine weapon code
		if (weaponName == "MINE") {
			if (weaponLevel == 1) {
				// Drop a stun mine
				Debug.Log("Level 1 (stun) mine dropped");
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [7], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<BoxCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				StartCoroutine (EnableMineCollision(tempPrefab.GetComponent<BoxCollider>(), player.GetComponent<VehicleControl>().shipCollider, 5f));
				tempPrefab.GetComponent<BoxCollider> ().enabled = true;
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
			if (weaponLevel == 2) {
				// Drop a holo-mine that effects both sides of the track
				Debug.Log("Level 2 (holo) mine dropped");
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [8], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<BoxCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				StartCoroutine (EnableMineCollision(tempPrefab.GetComponent<BoxCollider>(), player.GetComponent<VehicleControl>().shipCollider, 5f));
				tempPrefab.GetComponent<BoxCollider> ().enabled = true;
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
			if (weaponLevel == 3) {
				// Drop an explosive holo-mine with a larger activation radius
				Debug.Log("Level 3 (explosive-holo) mine dropped");
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [9], player.transform.position, player.transform.rotation);
				Physics.IgnoreCollision(tempPrefab.GetComponent<BoxCollider>(), player.GetComponent<VehicleControl>().shipCollider);
				StartCoroutine (EnableMineCollision(tempPrefab.GetComponent<BoxCollider>(), player.GetComponent<VehicleControl>().shipCollider, 5f));
				tempPrefab.GetComponent<BoxCollider> ().enabled = true;
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}
		} 

		// Eraser weapon code
		if (weaponName == "ERASER") {
			// Fire the Eraser orbital strike cannon at the first place player
			Debug.Log("Eraser orbital strike cannon activated!");

			// Find the target for the eraser using a loop
			for (int count = 0; count < gm.numberOfPlayers; count++) {
				if (gm.playerShipSelection [count].GetComponent<VehicleControl> ().currentPosition == 1) {
					target = gm.playerShipSelection [count];
				}
			}

			// If you are in first when you try and use the eraser, do nothing (give a debug error)
			if (target == player) {
				Debug.Log ("Cannot orbital strike yourself!");
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			} 
			// If someone else is in first, fire the eraser and reset the weapon
			else {
				tempPrefab = (GameObject)Instantiate (weaponPrefabs [10], target.transform.position, Quaternion.Euler(180f, target.transform.rotation.y, target.transform.rotation.z));
				tempPrefab.transform.parent = target.transform;
				tempPrefab.transform.Translate (1, -4, 0, Space.Self);
				Destroy (tempPrefab, 5.0f);
				player.GetComponent<VehicleControl> ().ResetWeapon ();
			}

		}
	}

	IEnumerator SpherePulseRadius(GameObject player, float size) {
		bool complete = false;

		while (!complete) {
			if (player.GetComponentInChildren<SphereCollider> ().radius < size) {
				player.GetComponentInChildren<SphereCollider> ().radius += 0.025f;
				yield return new WaitForSeconds (0.01f);
			} 
			else {
				complete = true;
			}
		}
	}

	IEnumerator CapsulePulseRadius(GameObject player, float size) {
		bool complete = false;

		while (!complete) {
			if (player.GetComponentInChildren<CapsuleCollider> ().radius < size) {
				player.GetComponentInChildren<CapsuleCollider> ().radius += 0.025f;
				yield return new WaitForSeconds (0.01f);
			} 
			else {
				complete = true;
			}
		}
	}

	IEnumerator EnableMineCollision(Collider col1, Collider col2, float time){
		yield return new WaitForSeconds (time);
		Physics.IgnoreCollision(col1, col2, false);		
	}

}
