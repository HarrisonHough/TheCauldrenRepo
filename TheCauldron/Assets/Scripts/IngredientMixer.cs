using UnityEngine;
using System.Collections;

public class IngredientMixer : MonoBehaviour {

  // Ingredient List
  public Transform Ham;

  // Recipe for testing
  public string PigReceipe = "Ham,Cheese";
  public string RoastRecipe = "Pig,Apple";

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(KeyCode.Space)) // FIXME: Just using spacebar to trigger
     {
         var go = Instantiate(Ham, transform.position + new Vector3(0, 3, 0), transform.rotation);
     }
	}
}
