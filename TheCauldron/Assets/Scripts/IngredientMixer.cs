using UnityEngine;
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

  private GameObject Projectile;
  public GameObject Target;
  private float waitProjectile;

	// Use this for initialization
	void Start () {
    ingredients = new ArrayList();
    waitProjectile = 1.0f;
    Projectile = null;
	}

  public void AddIngredient (string ingredient) {
    ingredients.Add(ingredient);
    // Debug.Log("=====");
    // foreach (string s in ingredients) {
    //   Debug.Log(s.ToString());
    // }
    Projectile = MixIngredient();
    if (Projectile) {
      Projectile.GetComponent<Rigidbody>().AddForce(new Vector3(100, Random.Range(200, 400), Random.Range(-200, 200)));
    }
  }

  GameObject MixIngredient () {
    if (ingredients.Contains("Ham") && ingredients.Contains("Cheese")) {// Array Contains required ingredient
      return SpawnIngredient(Pig);
    }
    return null;
  }

  GameObject SpawnIngredient (GameObject ingredient) {
    // Play cauldron flashy smoke effect
    var particleSystems = GetComponentsInChildren<ParticleSystem>();
    foreach (ParticleSystem particles in particleSystems) {
      particles.Play();
    }
    ingredients.Clear();
    return (GameObject) Instantiate(Pig, transform.position + new Vector3(0, 1.5f, 0), transform.rotation * Quaternion.Euler(Random.Range(-10,10),0,0));
  }

	// Update is called once per frame
	void Update () {
    if (waitProjectile > 0 && Projectile) {
      waitProjectile -= Time.deltaTime;
      if (waitProjectile >= 0) {
        Projectile.GetComponent<Ingredient>().FlyTo(Target.transform.position);
        waitProjectile = 1.0f;
        Projectile = null;
      }
    }
    if (Input.GetKeyDown(KeyCode.Space)) // DUMMY: Just using spacebar to trigger for testing
    {
      // if (Projectile)
      // Projectile.GetComponent<Ingredient>().FlyTo(Target.transform.position);
    }
	}
}
