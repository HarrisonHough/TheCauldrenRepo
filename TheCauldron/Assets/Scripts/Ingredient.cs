using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour {

  public string IngredientName;

  private GameObject cauldron;

	// Use this for initialization
	void Start () {
    cauldron = GameObject.Find("Cauldron");
	}

  void OnTriggerEnter(Collider other) {
    cauldron.GetComponent<IngredientMixer>().AddIngredient(IngredientName);
    Destroy(gameObject);
  }	

	// Update is called once per frame
	void Update () {

	}
}
