﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//should randomize the speed..
	//public float speed = 2f;
	public float distanceToPlayerForDeath = 1f;
	public GameObject player;
	public bool alive = true;
	public float speed;

	void Start() {
		speed = GenerateSpeed(EnemyLoader.level);
	}

	// Update is called once per frame
	public void Update () {
		transform.LookAt(player.transform.position);
		if (Vector3.Distance(player.transform.position, this.transform.position) >= distanceToPlayerForDeath) {
			//Move towards the character..
			transform.position += transform.forward * speed * Time.deltaTime;
		} else {
			//Game over or injury..
		}
	}

	float GenerateSpeed(int level) {
		//level 1 between 0.5 to 2
		if (level == 1) {
			return Random.Range(0.5f, 2f);
		}
		return Random.Range(2, 5);
	}
}
