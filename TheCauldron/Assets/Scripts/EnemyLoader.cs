using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLoader : MonoBehaviour {
	
	private float timeFromLastSpawn = -1;
	private float timeToNextSpawn = -1;
	public static int enemiesToSpawnThisLevel;

	public GameObject enemyPrefab;

	bool newGame = true;

	public void NewGame() {
		timeFromLastSpawn = Time.time;
		SetEnemiesToSpawnThisLevel();
		GenerateTimeToNextSpawn();
	}

	// Use this for initialization
	void Start () {
		NewGame();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.gameOver && enemiesToSpawnThisLevel > 0) {
			if (Time.time >= timeFromLastSpawn + timeToNextSpawn) {
				// spawn a new monster, regenerate the time to next spawn..
				GameObject enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity) as GameObject;
				if (GameManager.soundEffectsMuted) {
					enemy.GetComponent<AudioSource>().mute = true;
				}
				enemiesToSpawnThisLevel--;
				GenerateTimeToNextSpawn();
				timeFromLastSpawn = Time.time;
			}
		}
	}

	void GenerateTimeToNextSpawn() {
		//level 1 between 8 - 10 seconds..
		if (GameManager.level == 1) {
			timeToNextSpawn = Random.Range(8, 11);
		} else {
			//TODO: scale according to level..
			timeToNextSpawn = Random.Range(5, 8);
		}
	}

	void SetEnemiesToSpawnThisLevel() {
		Debug.Log("enemies to spawn this level: " + GameManager.level);
		enemiesToSpawnThisLevel = GameManager.level;
	}


}
