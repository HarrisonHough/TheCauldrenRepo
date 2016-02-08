using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static bool gameOver = false;
	public static int level = 1;
	public static bool wonLevel = false;
	public GameObject room;
	public static bool musicMuted = false;
	public static bool soundEffectsMuted = false;

	// sound effects: public GameObject ;
	public float difficulty = 0f;

	void Start() {
	}

	void OnLevelWasLoaded() {
		room.GetComponent<AudioSource>().mute = musicMuted;
		ToggleSoundEffects(soundEffectsMuted);
	}

	public void ToggleSoundEffects(bool muted) {
		Debug.Log("toggle sound effects, muted? " + muted);
		GameObject.Find("Cauldron").GetComponent<AudioSource>().mute = muted;
		soundEffectsMuted = muted;
	}

	public static void SetGameOver(bool won) {
		gameOver = true;
		wonLevel = won;
		if (!won) {
			level = 1;
			SceneManager.LoadScene("Title");
		} else {

		}
	}

	void Update() {
		//update difficulty depending on how many enemies on scene
		int numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

		difficulty = numOfEnemies / 15f;
		if (GameObject.Find("Cauldron").GetComponent<AudioSource>().mute != soundEffectsMuted) {
			ToggleSoundEffects(soundEffectsMuted);
		}
	}

}
