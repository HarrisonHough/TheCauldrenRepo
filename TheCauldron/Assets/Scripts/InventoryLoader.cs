using UnityEngine;
using System.Collections;

public class InventoryLoader : MonoBehaviour {

  public GameObject Inventory;
  public Transform SpawnLocation;

	// Use this for initialization
	void Awake () {
    Spawn();
	}

  public void Spawn () {
    Instantiate(Inventory, SpawnLocation.position, SpawnLocation.rotation);
  }

	// Update is called once per frame
	void Update () {

	}
}
