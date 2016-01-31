using UnityEngine;
using System.Collections;

public class IngredientMixer : MonoBehaviour {

  // Cauldron
  public GameObject Soup;
  public Material SoupMaterial;

  // Combinable item
  public GameObject Pig;
  public GameObject DragonClaw;
  public GameObject Pizza;
  public GameObject Potion;

  // Particle Emitter
  public ParticleSystem FlashySmoke;
  public ParticleSystem MagicalSmoke;

  // DUMMY: Ingredient tracker
  private ArrayList ingredients;

  private ArrayList Projectiles;
  private GameObject Target;
  private float waitProjectile;

	// Use this for initialization
	void Start () {
    ingredients = new ArrayList();
    Projectiles = new ArrayList();
	}

  public void AddIngredient (string ingredient) {
    ingredients.Add(ingredient);
    MixIngredient();
    MagicalSmoke.Play();
  }

  void MixIngredient () {
    if (ingredients.Contains("Potion")) {
      ingredients.Remove("Potion");
      SpawnIngredient(Potion);
      FlashySmoke.Play();
    }
    else if (ingredients.Contains("Ham") && ingredients.Contains("Cheese")) {
      ingredients.Remove("Ham");
      ingredients.Remove("Cheese");
      SpawnIngredient(Pig);
      FlashySmoke.Play();
    }
    else if (ingredients.Contains("Eye") && ingredients.Contains("Potion")) {
      ingredients.Remove("Eye");
      ingredients.Remove("Potion");
      SpawnIngredient(DragonClaw);
      FlashySmoke.Play();
    }
    else if (ingredients.Contains("Pizza")) {
      ingredients.Remove("Pizza");
      SpawnIngredient(Pizza);
      SpawnIngredient(Pizza);
      SpawnIngredient(Pizza);
      FlashySmoke.Play();
    }
  }

  void SpawnIngredient (GameObject ingredient) {
    GameObject Projectile = (GameObject) Instantiate(ingredient, transform.position + new Vector3(0, 1, 0), transform.rotation * Quaternion.Euler(Random.Range(-10,10),0,0));
    Rigidbody rb = Projectile.GetComponent<Rigidbody>();
    rb.isKinematic = false;
    Projectiles.Add(Projectile);
  }

  void GetClosestEnemy () {
    GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");

		GameObject closestEnemy;
		Vector3 position;

    foreach(GameObject obj in go) {
			//TODO: find the closest enemy here
      Target = obj;
      return;
			//if (obj.transform.position
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

    for (int i = 0; i < Projectiles.Length; i++) {
      if (Target) {
        Projectile.GetComponent<Ingredient>().FlyTo(targetPos + new Vector3(0, 3, 0));
      }
      Projectiles.Remove(Projectiles[i]);
    }
	}
}
