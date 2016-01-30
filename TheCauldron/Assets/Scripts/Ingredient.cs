using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour {

  public string IngredientName;

  private GameObject cauldron;

	// Use this for initialization
	void Start () {
    cauldron = GameObject.Find("Cauldron");
	}

  void OnTriggerEnter(Collider other) {
    cauldron.GetComponent<IngredientMixer>().AddIngredient(IngredientName);
    Destroy(gameObject);
  }

  public void FlyTo(Vector3 target) {
    Vector3 direction = target - transform.position;
    direction.Normalize();
    Debug.Log(direction.ToString());
    GetComponent<Rigidbody>().AddForce(direction * 10);
  }

	// Update is called once per frame
	void Update () {

	}
}
