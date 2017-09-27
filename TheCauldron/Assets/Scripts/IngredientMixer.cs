using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private ArrayList ingredients;
    private ArrayList projectiles;


    // Use this for initialization
    void Start () {
        ingredients = new ArrayList();
        projectiles = new ArrayList();
    }

    public void AddIngredient(IngredientType ingredient)
    {
        if (ingredients == null && projectiles == null)
        {
            ingredients = new ArrayList();
            projectiles = new ArrayList();
        }
        ingredients.Add(ingredient);
        //GetComponent<AudioSource>().Play();
        SoundManager.Instance.PlayRandomAddItem();
        MixIngredient();
        MagicalSmoke.Play();
    }

    public void AddIngredient(string ingredient)
    {
        if (ingredients == null && projectiles == null)
        {
            ingredients = new ArrayList();
            projectiles = new ArrayList();
        }
        ingredients.Add(ingredient);
        //GetComponent<AudioSource>().Play();
        SoundManager.Instance.PlayRandomAddItem();
        MixIngredient();
        MagicalSmoke.Play();
    }

    void MixIngredient()
    {
        if (ingredients.Contains(IngredientType.Eye) && ingredients.Contains(IngredientType.Potion))
        {
            ingredients.Remove(IngredientType.Eye);
            ingredients.Remove(IngredientType.Potion);
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains(IngredientType.Potion))
        {
            ingredients.Remove(IngredientType.Potion);
            SpawnIngredient(Potion);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains(IngredientType.Ham) && ingredients.Contains(IngredientType.Cheese))
        {
            ingredients.Remove(IngredientType.Ham);
            ingredients.Remove(IngredientType.Cheese);
            SpawnIngredient(Pig);
            SpawnIngredient(Pig);
            SpawnIngredient(Pig);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains(IngredientType.Pizza))
        {
            ingredients.Remove(IngredientType.Pizza);
            SpawnIngredient(Pizza);
            SpawnIngredient(Pizza);
            SpawnIngredient(Pizza);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains(IngredientType.DragonClaw))
        {
            ingredients.Remove(IngredientType.DragonClaw);
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains(IngredientType.Pig))
        {
            ingredients.Remove(IngredientType.Pig);
            SpawnIngredient(Pig);
            FlashySmoke.Play();
        }
        else
        {

        }
    }

    void MixIngredientOLD()
    {
        if (ingredients.Contains("Eye") && ingredients.Contains("Potion"))
        {
            ingredients.Remove("Eye");
            ingredients.Remove("Potion");
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains("Potion"))
        {
            ingredients.Remove("Potion");
            SpawnIngredient(Potion);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains("Ham") && ingredients.Contains("Cheese"))
        {
            ingredients.Remove("Ham");
            ingredients.Remove("Cheese");
            SpawnIngredient(Pig);
            SpawnIngredient(Pig);
            SpawnIngredient(Pig);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains("Pizza"))
        {
            ingredients.Remove("Pizza");
            SpawnIngredient(Pizza);
            SpawnIngredient(Pizza);
            SpawnIngredient(Pizza);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains("DragonClaw"))
        {
            ingredients.Remove("DragonClaw");
            SpawnIngredient(DragonClaw);
            SpawnIngredient(DragonClaw);
            FlashySmoke.Play();
        }
        else if (ingredients.Contains("Pig"))
        {
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
        ShootProjectiles();
    }

    public void ShootProjectiles()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            Vector3 targetPos;
            if (GameManager.Instance.EnemyManager.ClosestEnemy != null)
            {
                targetPos = GameManager.Instance.EnemyManager.ClosestEnemy.transform.position;
            }
            else
            {
                targetPos = transform.position;
            }
            ((GameObject)projectiles[i]).GetComponent<Ingredient>().FlyTo(targetPos + new Vector3(0, 3, 0));
        }

        projectiles.Clear();
    }
}
