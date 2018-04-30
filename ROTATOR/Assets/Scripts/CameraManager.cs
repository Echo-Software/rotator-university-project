using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	// Public variables
	public Transform cameraHook, cameraFocus;
	public GameObject cameraFillPlane;
	public int cameraNumber;

	// Private variables
	private GameManager gm;
	private float smoothSpeed = 10f;
	private Quaternion fixedRotation;
	private Renderer fillSprite;
	private Color tempColor;

	void Start(){		
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		fillSprite = cameraFillPlane.GetComponent<Renderer> ();
		tempColor = fillSprite.material.color;
		ResetCameraPosition ();
	}

	void Update(){
		
	}

	void FixedUpdate() {
		if (cameraHook.parent.tag == "Player") {
			// Keeps the camera oriented to gameobjects attached to the ship and smooths the movement
			transform.forward = Vector3.Lerp (transform.forward, cameraHook.forward, smoothSpeed * Time.deltaTime);
			transform.position = Vector3.Lerp (transform.position, cameraHook.position, smoothSpeed * Time.deltaTime);
			// transform.up = Vector3.Lerp (transform.up, cameraHook.up, smoothSpeed * Time.deltaTime);
			transform.LookAt (cameraFocus, cameraHook.up);
		} 
		else {
			transform.LookAt (cameraFocus, cameraHook.up);
		}
	} 

	public void AssignCameraHooks(GameObject hook, GameObject focus){		
		cameraHook = hook.transform;
		cameraFocus = focus.transform;
	}

	public void CameraWhiteFlash(){	
		StartCoroutine ("GradualWhiteFlash");
	}

	public void ResetCameraPosition(){
		transform.position = cameraHook.position;
	}

	IEnumerator GradualWhiteFlash(){
		bool complete = false;
		bool faded = false;
		float alpha = 0;

		while (!complete) {
			while (!faded){
				tempColor.a = alpha / 15;
				fillSprite.material.color = tempColor;
				alpha++;
				yield return new WaitForSeconds (0.01f);

				if (alpha == 15) {
					faded = true;
					tempColor.a = 1f;
					fillSprite.material.color = tempColor;
				}
			} 

			yield return new WaitForSeconds (0.1f);

			while (faded) {
				tempColor.a = alpha / 15;
				fillSprite.material.color = tempColor;
				alpha--;
				yield return new WaitForSeconds (0.01f);

				if (alpha == 0) {
					faded = false;
					complete = true;
				}
			}
		}
		tempColor.a = 0;
		fillSprite.material.color = tempColor;
	}

}