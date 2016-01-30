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
  private GameObject Target;
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
      Rigidbody rb = Projectile.GetComponent<Rigidbody>();
      rb.isKinematic = false;
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
    return (GameObject) Instantiate(Pig, transform.position + new Vector3(0, 2, 0), transform.rotation * Quaternion.Euler(Random.Range(-10,10),0,0));
  }

  void GetClosestEnemy () {
    GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");

    foreach(GameObject obj in go) {
      Target = obj;
    }
  }

	// Update is called once per frame
	void Update () {
    GetClosestEnemy();

    if (waitProjectile > 0 && Projectile && Target) {
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
