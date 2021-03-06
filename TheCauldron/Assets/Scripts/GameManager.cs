﻿using UnityEngine;
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
	private static AudioSource levelCompleteSfx;

	private static AudioSource[] witchCackleSfx;
	private static AudioSource[] witchDeathSfx;

	private static AudioSource itemDropFloorSfx1, itemDropFloorSfx2, itemDropTableSfx;

	private static int enemiesKilled;


	void Start() {
		AudioSource[] audioSources = room.GetComponents<AudioSource>();
		foreach (AudioSource source in audioSources) {
			if (source.clip.name.Equals("witch music one")) {
				musicOne = source;
			} else if (source.clip.name.Equals("witch music two")) {
				musicTwo = source;
			} else if (source.clip.name.Equals("witch music three")) {
				musicThree = source;
			} else if (source.clip.name.Equals("Cauldren_ItemDrop_1")) {
				itemDropFloorSfx1 = source;
			}  else if (source.clip.name.Equals("Cauldren_ItemDrop_2")) {
				itemDropTableSfx = source;
			}  else if (source.clip.name.Equals("Cauldren_ItemDrop_3")) {
				itemDropFloorSfx2 = source;
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
			} else if (source.clip.name.Equals("Cauldren_UI_1")) {
				levelCompleteSfx = source;
			}
		}

		AudioSource[] playerSfxSources = GameObject.Find("Player").GetComponents<AudioSource>();
		witchCackleSfx = new AudioSource[3];
		witchDeathSfx = new AudioSource[3];
		foreach (AudioSource source in playerSfxSources) {
			if (source.clip.name.Equals("Cauldren_Witch_Cackle_1")) {
				witchCackleSfx[0] = source;
			} else if (source.clip.name.Equals("Cauldren_Witch_Cackle_2")) {
				witchCackleSfx[1] = source;
			} else if (source.clip.name.Equals("Cauldren_Witch_Cackle_3")) {
				witchCackleSfx[2] = source;
			} else if (source.clip.name.Equals("Cauldren_Witch_Die_1")) {
				witchDeathSfx[0] = source;
			} else if (source.clip.name.Equals("Cauldren_Witch_Die_2")) {
				witchDeathSfx[1] = source;
			} else if (source.clip.name.Equals("Cauldren_Witch_Die_3")) {
				witchDeathSfx[2] = source;
			}
		}
	}

	public static void PlayTableDropSfx() {
		if (!soundEffectsMuted) {
			itemDropTableSfx.Play();
		}
	}

	public static void PlayRandomFloorDropSfx() {
		int floorRandom = Random.Range(0, 2);
		if (!soundEffectsMuted) {
			if (floorRandom == 0) {
				itemDropFloorSfx1.Play();
			} else {
				itemDropFloorSfx2.Play();
			}
		}
	}

	public static void PlayRandomWitchDeath() {
		int witchDeath = Random.Range(0, 3);
		if (!soundEffectsMuted) {
			witchDeathSfx[witchDeath].Play();
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

	public static void PlayRandomWitchCackle() {
		int witchCackle = Random.Range(0, 3);
		if (!soundEffectsMuted) {
			witchCackleSfx[witchCackle].Play();
		}
	}

	public static void PlayLevelCompleteSfx() {
		if (!soundEffectsMuted) {
			levelCompleteSfx.Play();
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

	public static int GetEnemiesKilled() {
		return enemiesKilled;
	}

	public static void AddEnemiesKilled(int killed) {
		enemiesKilled += killed;
	}

	public static void ResetEnemiesKilled() {
		enemiesKilled = 0;
	}

}
