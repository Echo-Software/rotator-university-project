﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnButton : MonoBehaviour {

	// Private variables
	private MenuManager menu;

	// Public variables

	// Use this for initialization
	void Start () {
		menu = GameObject.Find ("MenuManager").GetComponent<MenuManager> ();
	}

	public void Return() {
		menu.ResetButtons();
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

}