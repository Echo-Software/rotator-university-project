using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	// Private variables
	private MenuManager menu;

	// Public variables
	public Slider volumeSlider;
	public Slider brightnessSlider;

	void Start(){
		menu = GameObject.Find ("MenuManager").GetComponent<MenuManager> ();
		volumeSlider.value = menu.GetVolume ();
		brightnessSlider.value = menu.GetBrightness ();
	}

	void Update(){
		menu.SetVolume (volumeSlider.value);
		menu.SetBrightness (brightnessSlider.value);
	}

}
