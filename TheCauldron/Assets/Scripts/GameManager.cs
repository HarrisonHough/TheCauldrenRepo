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

	public static AudioSource cauldronAmbienceSfx;
	public static AudioSource cauldronBubblingSfx;
	public static AudioSource[] cauldronAddItemSfx;
	private static AudioSource cauldronScrollSfx;


	void Start() {
		Debug.Log("start method");
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

		AudioSource[] cauldronSfxSources = GameObject.Find("Cauldron").GetComponents<AudioSource>();
		cauldronAddItemSfx = new AudioSource[5];
		foreach (AudioSource source in cauldronSfxSources) {
			if (source.clip.name.Equals("Cauldren_Ambience_loop")) {
				cauldronAmbienceSfx = source;
			} else if (source.clip.name.Equals("Cauldren_ItemAdd_1")) {
				cauldronAddItemSfx[0] = source;
			} else if (source.clip.name.Equals("Cauldren_ItemAdd_2")) {
				cauldronAddItemSfx[1] = source;
			} else if (source.clip.name.Equals("Cauldren_ItemAdd_3")) {
				cauldronAddItemSfx[2] = source;
			} else if (source.clip.name.Equals("Cauldren_ItemAdd_4")) {
				cauldronAddItemSfx[3] = source;
			} else if (source.clip.name.Equals("Cauldren_ItemAdd_5")) {
				cauldronAddItemSfx[4] = source;
			} else if (source.clip.name.Equals("Cauldren_Scroll")) {
				cauldronScrollSfx = source;
			} else if (source.clip.name.Equals("Cauldren_bubbling_loop")) {
				cauldronBubblingSfx = source;
			}
		}
	}

	public static void PlayRandomAddItemSfx() {
		int itemAdd = Random.Range(0, 5);
		if (!soundEffectsMuted) {
			cauldronAddItemSfx[itemAdd].Play();
		}
	}

	public static void PlayOpenScrollSfx() {
		if (!soundEffectsMuted) {
			cauldronScrollSfx.Play();
		}
	}

	void OnLevelWasLoaded() {
		foreach (AudioSource source in room.GetComponents<AudioSource>()) {
			source.mute = musicMuted;
		}
		ToggleSoundEffects(soundEffectsMuted);
	}

	public void ToggleSoundEffects(bool muted) {
		cauldronAmbienceSfx.mute = muted;
		cauldronBubblingSfx.mute = muted;
		cauldronScrollSfx.mute = muted;
		foreach (AudioSource source in cauldronAddItemSfx) {
			source.mute = muted;
		}
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
