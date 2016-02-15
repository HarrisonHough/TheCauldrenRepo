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

	private float prevDifficulty = 0f;
	private AudioSource musicOne;
	private AudioSource musicTwo;
	private AudioSource musicThree;


	void Start() {
		AudioSource[] audioSources = room.GetComponents<AudioSource>();
		foreach (AudioSource source in audioSources) {
			if (source.clip.name.Equals("witch music one")) {
				musicOne = source;
			} else if (source.clip.name.Equals("witch music two")) {
				musicTwo = source;
			} else if (source.clip.name.Equals("witch music three")) {
				musicThree = source;
			}
		}
	}

	void OnLevelWasLoaded() {
		//room.GetComponent<AudioSource>().mute = musicMuted;
		foreach (AudioSource source in room.GetComponents<AudioSource>()) {
			source.mute = musicMuted;
		}
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

		if (numOfEnemies == 0) {
			difficulty = 0f;
			if (prevDifficulty != difficulty) {
				prevDifficulty = difficulty;
				musicTwo.Stop();
				musicThree.Stop();
				musicOne.Play();
			}
		} else if (numOfEnemies > 2 && numOfEnemies < 4) {
			difficulty = 0.5f;
			if (prevDifficulty != difficulty) {
				prevDifficulty = difficulty;
				musicTwo.Play();
				musicThree.Stop();
				musicOne.Stop();
			}
		} else if (numOfEnemies >= 4) {
			difficulty = 1f;
			if (prevDifficulty != difficulty) {
				prevDifficulty = difficulty;
				musicTwo.Stop();
				musicThree.Play();
				musicOne.Stop();
			}
		}

		if (GameObject.Find("Cauldron").GetComponent<AudioSource>().mute != soundEffectsMuted) {
			ToggleSoundEffects(soundEffectsMuted);
		}
	}

}
