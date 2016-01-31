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
    if (!GameManager.gameOver) {
      if (other.gameObject.tag == "IngredientHit") {
        cauldron.GetComponent<IngredientMixer>().AddIngredient(IngredientName);
        soup.GetComponent<SoupColor>().NextColor = SoupColor;
        GetComponent<AudioSource>().Play();
        Destroy(gameObject);
      } else if (other.gameObject.tag == "Enemy") {
        if (!GetComponent<Rigidbody>().isKinematic)
          other.gameObject.GetComponent<Enemy>().OnHit();
      } else if (other.gameObject.tag == "Floor") {
        Destroy(gameObject);
      }
    }
  }

  public void FlyTo(Vector3 target) {
    Vector3 direction = target - transform.position;
    direction.Normalize();
    GetComponent<Rigidbody>().AddForce(direction * 500);
  }

	// Update is called once per frame
	void Update () {

	}
}
