using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	// Public variables
	public Transform cameraHook;
	public Transform cameraFocus;

	// Private variables
	private float smoothSpeed = 5f;
	
	// Update is called once per frame
	void FixedUpdate () {
		// Keeps the camera oriented to gameobjects attached to the ship and smooths the movement
		transform.forward = Vector3.Lerp (transform.forward, cameraHook.forward, smoothSpeed * Time.deltaTime);
		transform.position = Vector3.Lerp (transform.position, cameraHook.position, smoothSpeed * Time.deltaTime);
		transform.LookAt (cameraFocus, cameraHook.up);
	}

}