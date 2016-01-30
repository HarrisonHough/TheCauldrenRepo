using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour {

  public string IngredientName;
  public Color SoupColor;

  private GameObject cauldron;
  private GameObject soup;

	// Use this for initialization
	void Start () {
    cauldron = GameObject.Find("Cauldron");
    soup = GameObject.Find("Soup");
	}

  void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == "Enemy") {
      Destroy(other.gameObject);
			if (EnemyLoader.enemiesToSpawnThisLevel <= 0 && GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) {
				//you won!
				GameManager.SetGameOver(true);
			}
    } else if (other.gameObject.tag == "IngredientHit") {
      cauldron.GetComponent<IngredientMixer>().AddIngredient(IngredientName);
      soup.GetComponent<SoupColor>().NextColor = SoupColor;
      Destroy(gameObject);
    }

  }

  public void FlyTo(Vector3 target) {
    Vector3 direction = target - transform.position;
    direction.Normalize();
    Debug.Log(direction.ToString());
    GetComponent<Rigidbody>().AddForce(direction * 500);
  }

	// Update is called once per frame
	void Update () {

	}
}
