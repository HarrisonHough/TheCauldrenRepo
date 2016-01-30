using UnityEngine;
using System.Collections;

public class MenuPotion : MonoBehaviour {

	public string menuString;

	private GameObject cauldron;

	// Use this for initialization
	void Start () {
		cauldron = GameObject.Find("Cauldron");
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("removing from: " + menuString);
		cauldron.GetComponent<Menu>().CallMenuItem(menuString);
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
