using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAndBob : MonoBehaviour {

    private int degreesPs = 20;
    private float amplitude = 0.5f;
    private int frequency = 1;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

	// Use this for initialization
	void Start ()
    {
        posOffset = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Spins on the Y-axis
        transform.Rotate(new Vector3(0, Time.deltaTime * degreesPs, 0), Space.Self);

        // Bob up and down
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
