using UnityEngine;
using System.Collections;

public class IngredientMixer : MonoBehaviour {

  // Ingredient List
  public Transform Ham;
  public Transform Cheese;
  public Transform Pig;

  // Recipe for testing
  public string PigReceipe = "Ham,Cheese";
  public string RoastRecipe = "Pig,Apple";

	// Use this for initialization
	void Start () {

	}

  Object MixIngredient () {
    if (true) {// Array Contains required ingredient
      return Instantiate(Pig, transform.position + new Vector3(0, 3, 0), transform.rotation);
    }
  }

	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(KeyCode.Space)) // FIXME: Just using spacebar to trigger for testing
    {
      var newIngredient = MixIngredient();

    }
	}
}
