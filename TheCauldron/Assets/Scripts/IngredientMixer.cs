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

  // Particle Emitter
  public Transform FlashySmoke;

  // FIXME: Ingredient tracker
  private ArrayList ingredients;

	// Use this for initialization
	void Start () {
    ingredients = new ArrayList();
	}

  Object MixIngredient () {
    ingredients.Add("Ham");
    ingredients.Add("Cheese");
    Debug.Log(ingredients);
    if (ingredients.Contains("Ham") && ingredients.Contains("Cheese")) {// Array Contains required ingredient

      // Play cauldron flashy smoke effect
      var particleSystems = GetComponentsInChildren<ParticleSystem>();
      foreach (ParticleSystem particles in particleSystems) {
        particles.Play();
      }
      ingredients.Clear();
      return Instantiate(Pig, transform.position + new Vector3(0, 1, 0), transform.rotation);

    }
    return null;
  }

	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(KeyCode.Space)) // FIXME: Just using spacebar to trigger for testing
    {
      var newIngredient = MixIngredient();
    }
	}
}
