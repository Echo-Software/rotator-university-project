using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

	// Public Variables

	// Private Variables
	private GameManager gm;
	private string[] weaponTypes = new string[] {"PULSE","MISSILE","SHIELD","MINE","ERASER"};

	void Start(){
		gm = gameObject.GetComponent<GameManager> ();
	}

	public string RandomWeapon(int currentPosition){
		// Check if the player is in last position before deciding which weapon they get (only last place can get the Eraser)
		if (currentPosition == gm.numberOfPlayers) {
			return weaponTypes [Random.Range (0, 5)];
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

}
