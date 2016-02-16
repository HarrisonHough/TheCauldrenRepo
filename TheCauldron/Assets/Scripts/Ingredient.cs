using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour
{

	public string IngredientName;
	public Color SoupColor;

	private GameObject cauldron;
	private GameObject soup;

	// Use this for initialization
	void Start()
	{
		cauldron = GameObject.Find("Cauldron");
		soup = GameObject.Find("Soup");
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log("collision! " + collision.collider.name);
		if (collision.collider.tag.Equals("Floor")) {
			Debug.Log("floor!");
			GameManager.PlayRandomFloorDropSfx();
		} else if (collision.collider.tag.Equals("Table")) {
			Debug.Log("table!");
			GameManager.PlayTableDropSfx();
		}
	}


	void OnTriggerEnter(Collider other)
	{
		Debug.Log("triggered.." + other.name);
		if (!GameManager.gameOver) {
			if (other.gameObject.tag == "IngredientHit") {
				cauldron.GetComponent<IngredientMixer>().AddIngredient(IngredientName);
				soup.GetComponent<SoupColor>().NextColor = SoupColor;
				Destroy(gameObject);
			} else if (other.gameObject.tag == "Enemy") {
				if (!GetComponent<Rigidbody>().isKinematic) {
					other.gameObject.GetComponent<Enemy>().OnHit();
					GameManager.PlayLevelCompleteSfx();
				}
			} /*else if (other.gameObject.tag == "Floor") {
				Destroy(gameObject);
			}*/
		}
	}

	public void FlyTo(Vector3 target)
	{
		Vector3 direction = target - transform.position;
		direction.Normalize();
		GetComponent<Rigidbody>().AddForce(direction * 500);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
