﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	// Public variables
	public Transform cameraHook, cameraFocus;
	public GameObject cameraFillPlane;

	// Private variables
	private float smoothSpeed = 5f;
	private Quaternion fixedRotation;
	private Renderer fillSprite;
	private Color tempColor;

	void Start(){		
		fillSprite = cameraFillPlane.GetComponent<Renderer> ();
		transform.position = cameraHook.position;
		tempColor = fillSprite.material.color;
	}

	void Update(){
		
	}

	void FixedUpdate() {
		// Keeps the camera oriented to gameobjects attached to the ship and smooths the movement
		transform.forward = Vector3.Lerp (transform.forward, cameraHook.forward, smoothSpeed * Time.deltaTime);
		transform.position = Vector3.Lerp (transform.position, cameraHook.position, smoothSpeed * Time.deltaTime);
		// transform.up = Vector3.Lerp (transform.up, cameraHook.up, smoothSpeed * Time.deltaTime);
		transform.LookAt (cameraFocus, cameraHook.up);
	} 

	public void CameraWhiteFlash(){	
		StartCoroutine ("GradualWhiteFlash");
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