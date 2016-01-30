using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardboardInteract : MonoBehaviour {

	public ParticleSystem SelectedItemParticles;
	public GameObject musicItemButton;
	public Text musicItemText;
	public GameObject soundEffectsItemButton;
	public Text soundEffectsItemText;
	public GameObject creditsItemButton;
	public Text creditsItemText;
	public Text gameOverText;
	GameObject[] items;
	bool holdingItem;
	GameObject itemHeld;
	float timeInteracted;
	float distance = 1;
	float smooth = 4;
	float turnspeed = 1;

	// Use this for initialization
	void Start () {
		items = GameObject.FindGameObjectsWithTag("Item");
	}

	// Update is called once per frame
	void Update () {
		
		if (GameObject.Find("PaperMenu") != null && !holdingItem) {
			gameOverText.gameObject.SetActive(false);
			if ((VRDevice.family != "oculus" && Cardboard.SDK.Triggered) || Input.GetMouseButtonUp(0)) {
				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)) {
					if (hit.collider.gameObject == musicItemButton && Time.time > timeInteracted + 0.3f) {
						timeInteracted = Time.time;
						Debug.Log("music item!");
						bool enabled = musicItemText.fontStyle == FontStyle.Normal;
						if (enabled) {
							//disable..
							musicItemText.fontStyle = FontStyle.Italic;
							musicItemText.color = new Color(28/255f, 28/255f, 28/255f, 90/255f); //1C1C1CFF
							Debug.Log("disable");
							GameObject.Find("Room").GetComponent<AudioSource>().mute = true;
							//GameManager.musicEnabled(false);
							//disable music
						} else {
							//enable..
							musicItemText.fontStyle = FontStyle.Normal;
							musicItemText.color = new Color(28/255f, 28/255f, 28/255f, 1); //1C1C1CFF
							Debug.Log("enable");
							GameObject.Find("Room").GetComponent<AudioSource>().mute = false;
							//GameManager.musicEnabled(true);
							//enable music..
						}
					} else if (hit.collider.gameObject == soundEffectsItemButton && Time.time > timeInteracted + 0.3f) {
						timeInteracted = Time.time;
						Debug.Log("sound effects item!");
						bool enabled = soundEffectsItemText.fontStyle == FontStyle.Normal;
						if (enabled) {
							//disable..
							soundEffectsItemText.fontStyle = FontStyle.Italic;
							soundEffectsItemText.color = new Color(28/255f, 28/255f, 28/255f, 90/255f); //1C1C1CFF
							Debug.Log("disable");
							//disable sound effects
						} else {
							//enable..
							soundEffectsItemText.fontStyle = FontStyle.Normal;
							soundEffectsItemText.color = new Color(28/255f, 28/255f, 28/255f, 1); //1C1C1CFF
							Debug.Log("enable");
							//enable sound effects..
						}
						//TODO: do something for sound effects..
					} else if (hit.collider.gameObject == creditsItemButton && Time.time > timeInteracted + 0.3f) {
						timeInteracted = Time.time;
						Debug.Log("credits item!");
						bool enabled = creditsItemText.fontStyle == FontStyle.Normal;
						if (enabled) {
							//disable..
							creditsItemText.fontStyle = FontStyle.Italic;
							creditsItemText.color = new Color(28/255f, 28/255f, 28/255f, 90/255f); //1C1C1CFF
							Debug.Log("disable");
							//disable music
						} else {
							//enable..
							creditsItemText.fontStyle = FontStyle.Normal;
							creditsItemText.color = new Color(28/255f, 28/255f, 28/255f, 1); //1C1C1CFF
							Debug.Log("enable");
							//enable music..
						}
						//TODO: do something for credits..
					}
				}
			}
		}

		if (!GameManager.gameOver) {
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

					if (holdingItem && Time.time > timeInteracted + 0.3f) {
						holdingItem = false;
						itemHeld.gameObject.GetComponent<Rigidbody>().isKinematic = false;
						SelectedItemParticles.transform.position = new Vector3(0, -10, 0);
						itemHeld = null;
						timeInteracted = Time.time;
					}
				}
			}

			if (holdingItem && itemHeld != null) {
				Vector3 frontOfCamera = Camera.main.transform.position + Camera.main.transform.forward * distance;
				Vector3 diffVector = frontOfCamera - itemHeld.transform.position;
				float diff = diffVector.magnitude;
				itemHeld.transform.position = Vector3.Lerp(itemHeld.transform.position, frontOfCamera, Time.deltaTime * smooth);
				SelectedItemParticles.transform.position = itemHeld.transform.position;
				itemHeld.transform.rotation *= Quaternion.Euler(Random.Range(5, 10) * diff * (diffVector.z / Mathf.Abs(diffVector.z)), 0, 0);
			}
		} /*else if (GameObject.Find("PaperMenu") == null) {// end of gameover
			gameOverText.gameObject.SetActive(true);
		}*/
		if (SceneManager.GetActiveScene().name.Equals("Title") && GameManager.gameOver) {
			gameOverText.gameObject.SetActive(true);
			GameManager.gameOver = false;
		}
	}
}
