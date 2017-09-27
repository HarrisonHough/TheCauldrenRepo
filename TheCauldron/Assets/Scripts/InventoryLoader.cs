using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLoader : MonoBehaviour {

    public GameObject Inventory;
    public Transform SpawnLocation;
    private GameObject lastInventory;

    // Use this for initialization
    void Awake()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (lastInventory)
        {
            Destroy(lastInventory);
        }
        lastInventory = (GameObject)Instantiate(Inventory, SpawnLocation.position, SpawnLocation.rotation);
    }
}
