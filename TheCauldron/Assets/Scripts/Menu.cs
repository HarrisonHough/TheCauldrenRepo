using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public GameObject paperMenu;

	// Use this for initialization
	void Start () {
	}

	public void CallMenuItem(string action) {
		if (action.Equals("Play")) {
			//play screen
			Debug.Log("Play game!");
			SceneManager.LoadScene("Main");
		} else if (action.Equals("Options")) {
			//options screen
			Debug.Log("Options");
			paperMenu.SetActive(true);
			GameManager.PlayOpenScrollSfx();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
