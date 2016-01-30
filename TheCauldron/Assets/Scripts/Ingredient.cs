using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour {

  public string IngredientName;
  // public bool spawning; // Animation toggle for spawning
  // public float spawningTimer; // Animation duration

	// Use this for initialization
	void Start () {
    // spawning = false;
    // spawningTimer = 0.0f;
	}

  // public void PlaySpawnAnimation () {
  //   spawning = true;
  //   spawningTimer = 3.0f;
  // }

	// Update is called once per frame
	void Update () {
    // if (spawning) {
    //   Debug.Log(spawningTimer);
    //   Debug.Log(Time.deltaTime);
    //   transform.Translate(Vector3.up * Time.deltaTime);
    //   spawningTimer -= Time.deltaTime;
    //   if (spawningTimer <= 0.0f) {
    //     spawning = false;
    //   }
    // }
	}
}
