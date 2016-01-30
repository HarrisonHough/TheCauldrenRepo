using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLoader : MonoBehaviour {
	
	private float timeFromLastSpawn = -1;
	private float timeToNextSpawn = -1;

	public static int level = 1;
	public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= timeFromLastSpawn + timeToNextSpawn) {
			// spawn a new monster, regenerate the time to next spawn..
			Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
			GenerateTimeToNextSpawn();
			timeFromLastSpawn = Time.time;
		}
	}

	void GenerateTimeToNextSpawn() {
		//level 1 between 8 - 10 seconds..
		if (level == 1) {
			timeToNextSpawn = Random.Range(8, 11);
		} else {
			//TODO: scale according to level..

		}
	}
}
