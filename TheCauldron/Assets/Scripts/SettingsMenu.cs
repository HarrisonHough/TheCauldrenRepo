using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour {

    public GameObject paperMenu;

	// Use this for initialization
	void Start () {
		
	}

    public void ShowSettingsMenu()
    {
        paperMenu.SetActive(true);
    }

    public void HideSettingsMenu()
    {
        paperMenu.SetActive(false);
    }
}
