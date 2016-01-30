using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class CardboardInteract : MonoBehaviour {

	public bool CardboardEnabled;
	GameObject[] items;
	bool holdingItem;
	GameObject itemHeld;
	float timeInteracted;
	float distance = 1;
	float smooth = 4;

	// Use this for initialization
	void Start () {
		items = GameObject.FindGameObjectsWithTag("Item");
	}

	// Update is called once per frame
	void Update () {

		if ((VRDevice.family != "oculus" && Cardboard.SDK.Triggered) || Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				if (!holdingItem && Time.time > timeInteracted + 0.3f) {

					foreach (GameObject item in items) {
						if (hit.collider.gameObject == item) {
							holdingItem = true;
							itemHeld = item;
							timeInteracted = Time.time;
						}
					}
				}
			}

			if (holdingItem && Time.time > timeInteracted + 0.3f) {
				holdingItem = false;
				itemHeld.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				itemHeld = null;
				timeInteracted = Time.time;
			}
		}

		if (holdingItem && itemHeld != null) {
			itemHeld.transform.position = Vector3.Lerp(itemHeld.transform.position, Camera.main.transform.position + Camera.main.transform.forward * distance, Time.deltaTime * smooth);
		}
	}
}
