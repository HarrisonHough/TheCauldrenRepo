using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	GameObject cauldron;

	// Use this for initialization
	void Start () {
		cauldron = GameObject.FindGameObjectWithTag("Cauldron");
	}

	public void CallMenuItem(string action) {
		if (action.Equals("Play")) {
			//play screen
			Debug.Log("Play game!");
		} else if (action.Equals("Options")) {
			//options screen
			Debug.Log("Options");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
