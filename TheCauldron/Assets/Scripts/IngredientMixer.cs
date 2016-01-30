﻿using UnityEngine;
using System.Collections;

public class IngredientMixer : MonoBehaviour {

  // Ingredient List
  public GameObject Ham;
  public GameObject Cheese;
  public GameObject Pig;

  // Recipe for testing
  public string PigReceipe = "Ham,Cheese";
  public string RoastRecipe = "Pig,Apple";

  // Particle Emitter
  public Transform FlashySmoke;

  // DUMMY: Ingredient tracker
  private ArrayList ingredients;

	// Use this for initialization
	void Start () {
    ingredients = new ArrayList();
	}

  GameObject MixIngredient () {

    // DUMMY: Ingredient
    ingredients.Add("Ham");
    ingredients.Add("Cheese");
    if (ingredients.Contains("Ham") && ingredients.Contains("Cheese")) {// Array Contains required ingredient

      // Play cauldron flashy smoke effect
      var particleSystems = GetComponentsInChildren<ParticleSystem>();
      foreach (ParticleSystem particles in particleSystems) {
        particles.Play();
      }
      ingredients.Clear();
      return (GameObject) Instantiate(Pig, transform.position + new Vector3(0, 1, 0), transform.rotation * Quaternion.Euler(Random.Range(-10,10),0,0));
    }
    return null;
  }

	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(KeyCode.Space)) // DUMMY: Just using spacebar to trigger for testing
    {
      GameObject obj = MixIngredient();
      if (obj) {
        obj.GetComponent<Rigidbody>().AddForce(new Vector3(100, Random.Range(200, 400), Random.Range(-100, 100)));
      }
    }
	}
}
