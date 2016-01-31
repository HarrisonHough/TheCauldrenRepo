﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLoader : MonoBehaviour {
	
	private float timeFromLastSpawn = -1;
	private float timeToNextSpawn = -1;
	public static int enemiesToSpawnThisLevel;

	public GameObject enemyPrefab;

	public void NewGame() {
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
				Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
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

	void ToggleNorthSpawner(bool toggle) {
		GameObject.Find("EnemySpawnerN").gameObject.SetActive(toggle);
	}

	void ToggleSouthSpawner(bool toggle) {
		GameObject.Find("EnemySpawnerS").gameObject.SetActive(toggle);
	}

	void ToggleEastSpawner(bool toggle) {
		GameObject.Find("EnemySpawnerE").gameObject.SetActive(toggle);
	}

	void ToggleWestSpawner(bool toggle) {
		GameObject.Find("EnemySpawnerW").gameObject.SetActive(toggle);
	}

	void SetEnemiesToSpawnThisLevel() {
		if (GameManager.level == 1) {
			//only three enemies in level 1..
			enemiesToSpawnThisLevel = 3;
			//only the North spawner is open..
			ToggleEastSpawner(false);
			ToggleNorthSpawner(true);
			ToggleSouthSpawner(false);
			ToggleWestSpawner(false);
		} else if (GameManager.level == 2) {
			enemiesToSpawnThisLevel = 4;
			// the North & South spawner is open..
			ToggleEastSpawner(false);
			ToggleNorthSpawner(true);
			ToggleSouthSpawner(true);
			ToggleWestSpawner(false);
		} else {
			enemiesToSpawnThisLevel = 2;
		}
		//TODO: set enemies we can spawn per level.. and maybe spawners??
	}


}
