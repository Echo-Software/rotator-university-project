using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject subMenu;
    private bool uiStatus = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        PauseMenu();
    }

    void PauseMenu()
    {
        if (Input.GetButtonDown("Player1_Start_Button") && uiStatus == false)
        {
			Time.timeScale = 0;
            uiStatus = true;
            subMenu.gameObject.SetActive(true);
        }
		else if (Input.GetButtonDown("Player1_Start_Button") && uiStatus == true)
        {
			Time.timeScale = 1;
            uiStatus = false;
            subMenu.gameObject.SetActive(false);
        }
    }
    
}
