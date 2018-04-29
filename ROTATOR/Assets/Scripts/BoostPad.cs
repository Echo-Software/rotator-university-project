using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour {

    private int speed = 2;
    private Material padM;

	// Use this for initialization
	void Start ()
    {
        padM = GetComponent<Renderer>().material;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 offset = new Vector2(-Time.time * speed % 1, 0);
        padM.SetTextureOffset("_MainTex", offset);	
	}
}
