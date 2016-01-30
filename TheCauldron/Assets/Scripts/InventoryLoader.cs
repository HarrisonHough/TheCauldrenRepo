using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

  public Transform SpawnLocation;

	// Use this for initialization
	void Start () {
    Spawn();
	}

  public void Spawn () {
    Instantiate(gameObject, SpawnLocation.position, SpawnLocation.rotation);
  }

	// Update is called once per frame
	void Update () {

	}
}
