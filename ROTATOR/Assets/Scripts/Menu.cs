using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject subMenu;
    bool uiStatus = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        menu();
    }

    void menu()
    {
        if (Input.GetButtonDown("Start_Button") && uiStatus == false)
        {
            uiStatus = true;
            subMenu.gameObject.SetActive(true);
        }
        else if (Input.GetButtonDown("Start_Button") && uiStatus == true)
        {
            uiStatus = false;
            subMenu.gameObject.SetActive(false);
        }
    }
}
