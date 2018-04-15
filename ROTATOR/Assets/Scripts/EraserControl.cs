using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserControl : MonoBehaviour {

	// Public variables
	public BoxCollider box;
	public GameObject orbitalCannon;

	// Private variables
	private float timer;
	private bool fired = false;
	private GameObject target;

	// Use this for initialization
	void Start(){
		
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		orbitalCannon.transform.Rotate (Vector3.up * 360 * Time.deltaTime);

		if (timer > 3 && !fired) {
			target.GetComponent<VehicleControl> ().invincible = false;
			box.enabled = true;
		}
	}

	// Keep the eraser following it's target 
	void FixedUpdate (){
		transform.position = Vector3.MoveTowards (transform.position, target.transform.position, 100 * Time.deltaTime);
		transform.up = -target.transform.up;
	}

	public void ObtainTarget (GameObject eraserTarget){
		target = eraserTarget;
	}

}
