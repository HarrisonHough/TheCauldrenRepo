using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IngredientMixer : MonoBehaviour
{

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

	private ArrayList ingredients;
	private ArrayList projectiles;

	// Use this for initialization
	void Start()
	{
		ingredients = new ArrayList();
		projectiles = new ArrayList();
	}

	public void AddIngredient(string ingredient)
	{
		if (ingredients == null && projectiles == null) {
			ingredients = new ArrayList();
			projectiles = new ArrayList();
		}
		ingredients.Add(ingredient);
		//GetComponent<AudioSource>().Play();
		GameManager.PlayRandomAddItemSfx();
		MixIngredient();
		MagicalSmoke.Play();
	}

	void MixIngredient()
	{
		if (ingredients.Contains("Eye") && ingredients.Contains("Potion")) {
			ingredients.Remove("Eye");
			ingredients.Remove("Potion");
			SpawnIngredient(DragonClaw);
			SpawnIngredient(DragonClaw);
			SpawnIngredient(DragonClaw);
			SpawnIngredient(DragonClaw);
			SpawnIngredient(DragonClaw);
			FlashySmoke.Play();
		} else if (ingredients.Contains("Potion")) {
			ingredients.Remove("Potion");
			SpawnIngredient(Potion);
			FlashySmoke.Play();
		} else if (ingredients.Contains("Ham") && ingredients.Contains("Cheese")) {
			ingredients.Remove("Ham");
			ingredients.Remove("Cheese");
			SpawnIngredient(Pig);
			SpawnIngredient(Pig);
			SpawnIngredient(Pig);
			FlashySmoke.Play();
		} else if (ingredients.Contains("Pizza")) {
			ingredients.Remove("Pizza");
			SpawnIngredient(Pizza);
			SpawnIngredient(Pizza);
			SpawnIngredient(Pizza);
			FlashySmoke.Play();
		} else if (ingredients.Contains("DragonClaw")) {
			ingredients.Remove("DragonClaw");
			SpawnIngredient(DragonClaw);
			SpawnIngredient(DragonClaw);
			FlashySmoke.Play();
		} else if (ingredients.Contains("Pig")) {
			ingredients.Remove("Pig");
			SpawnIngredient(Pig);
			FlashySmoke.Play();
		}
	}

	void SpawnIngredient(GameObject ingredient)
	{
		GameObject Projectile = (GameObject)Instantiate(ingredient, transform.position + new Vector3(0, 1, 0), transform.rotation * Quaternion.Euler(Random.Range(-10, 10), 0, 0));
		Rigidbody rb = Projectile.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		projectiles.Add(Projectile);
	}

	GameObject[] GetClosestEnemies(int numberOfEnemies)
	{
		List<GameObject> allEnemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
		Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

		int min = Mathf.Min(allEnemies.Count, numberOfEnemies);
		GameObject[] closestXEnemies = new GameObject[min];

		for (int i = 0; i < min; i++) {
			foreach (GameObject enemy in allEnemies) {
				if (closestXEnemies[i] == null) {
					closestXEnemies[i] = enemy;
				} else {
					if (Vector3.Distance(enemy.transform.position, playerPos) < Vector3.Distance(closestXEnemies[i].transform.position, playerPos)) {
						closestXEnemies[i] = enemy;
					}
				}
			}
			allEnemies.Remove(closestXEnemies[i]);
		}

		return closestXEnemies;
	}

	GameObject GetClosestEnemy()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");

		if (go.Length > 0) {
			return GetClosestEnemies(1)[0];
		}
		return null;
	}

	// Update is called once per frame
	void Update()
	{
		GameObject[] enemies = GetClosestEnemies(projectiles.Count);

		for (int i = 0; i < projectiles.Count; i++) {
			Vector3 targetPos;
			if (i < enemies.Length) {
				targetPos = enemies[i].transform.position;
			} else {
				targetPos = transform.position;
			}
			((GameObject)projectiles[i]).GetComponent<Ingredient>().FlyTo(targetPos + new Vector3(0, 3, 0));
		}

		projectiles.Clear();
	}

	public GameObject[] GetAllEnemies()
	{
		return GameObject.FindGameObjectsWithTag("Enemy");
	}
}
