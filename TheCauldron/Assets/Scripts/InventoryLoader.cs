using UnityEngine;
using System.Collections;

public class InventoryLoader : MonoBehaviour {

  public GameObject Inventory;
  public Transform SpawnLocation;
  private GameObject lastInventory;

	// Use this for initialization
	void Awake () {
    Spawn();
	}

  public void Spawn () {
    if (lastInventory) {
      Destroy(lastInventory);
    }
    lastInventory = (GameObject) Instantiate(Inventory, SpawnLocation.position, SpawnLocation.rotation);
  }

	// Update is called once per frame
	void Update () {

	}
}
