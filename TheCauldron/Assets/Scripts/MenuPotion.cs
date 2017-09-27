using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuPotion : MonoBehaviour {

    public enum MenuOption {Play, Settings }
    public MenuOption menuOption;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cauldron")
        {
            MenuFunction();
            //hide object
            gameObject.SetActive(false);
        }
    }

    
    private void MenuFunction()
    {
        // TODO add menu functions
        switch (menuOption)
        {
            case MenuOption.Play:
                // Load game level
                break;
            case MenuOption.Settings:
                // SHow Menu
                break;
            default:
                break;

        }
    }
}
