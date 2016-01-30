﻿using UnityEngine;
using System.Collections;

public class IngredientMixer : MonoBehaviour {

  // Cauldron
  public GameObject Soup;
  public Material SoupMaterial;

  // Combinable item
  public GameObject Pig;
  public GameObject DragonClaw;
  public GameObject Pizza;

  // Particle Emitter
  public ParticleSystem FlashySmoke;
  public ParticleSystem MagicalSmoke;

  // DUMMY: Ingredient tracker
  private ArrayList ingredients;

  private GameObject Projectile;
  private GameObject Target;
  private float waitProjectile;

	// Use this for initialization
	void Start () {
    ingredients = new ArrayList();
    Projectile = null;
	}

  public void AddIngredient (string ingredient) {
    ingredients.Add(ingredient);
    Projectile = MixIngredient();
    MagicalSmoke.Play();
    if (Projectile) {
      Rigidbody rb = Projectile.GetComponent<Rigidbody>();
      rb.isKinematic = false;
    }
  }

  GameObject MixIngredient () {
    if (ingredients.Contains("Ham") && ingredients.Contains("Cheese")) {
      ingredients.Remove("Ham");
      ingredients.Remove("Cheese");
      return SpawnIngredient(Pig);
    }
    if (ingredients.Contains("Eye") && ingredients.Contains("Potion")) {
      ingredients.Remove("Eye");
      ingredients.Remove("Potion");
      return SpawnIngredient(DragonClaw);
    }
    if (ingredients.Contains("Potion") && ingredients.Contains("Pizza")) {
      ingredients.Remove("Pizza");
      ingredients.Remove("Potion");
      return SpawnIngredient(Pizza);
    }
    return null;
  }

  GameObject SpawnIngredient (GameObject ingredient) {
    FlashySmoke.Play();
    ingredients.Clear();
    return (GameObject) Instantiate(ingredient, transform.position + new Vector3(0, 1, 0), transform.rotation * Quaternion.Euler(Random.Range(-10,10),0,0));
  }

  void GetClosestEnemy () {
    GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");

    foreach(GameObject obj in go) {
      Target = obj;
      return;
    }
  }

	// Update is called once per frame
	void Update () {
    GetClosestEnemy();
    Vector3 targetPos;
    if (!Target) {
      targetPos = transform.position;
    } else {
      targetPos = Target.transform.position;
    }

    if (Projectile && Target) {
      Projectile.GetComponent<Ingredient>().FlyTo(targetPos + new Vector3(0, 3, 0));
      Projectile = null;
    }
    if (Input.GetKeyDown(KeyCode.Space)) // DUMMY: Just using spacebar to trigger for testing
    {
      // if (Projectile)
      // Projectile.GetComponent<Ingredient>().FlyTo(Target.transform.position);
    }
	}
}
