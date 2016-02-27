using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

	//should randomize the speed..
	//public float speed = 2f;
	public float distanceToPlayerForDeath = 1f;
	public GameObject player;
	public GameObject DeadEnemyPrefab;
	public GameObject LoseSound;

	public bool alive = true;
	public float speed;

	void Start()
	{
		speed = GenerateSpeed(GameManager.level);
	}

	public void OnHit()
	{
		GameManager.AddEnemiesKilled(1);
		Instantiate(DeadEnemyPrefab, transform.position, transform.rotation);
		if (EnemyLoader.enemiesToSpawnThisLevel <= 0 && GameObject.FindGameObjectsWithTag("Enemy").Length <= 1) {
			//you won!
			GameManager.SetGameOver(true);
			GameManager.PlayRandomWitchCackle();
		}
		Destroy(gameObject);
	}

	// Update is called once per frame
	public void Update()
	{
		if (!GameManager.gameOver) {
			transform.LookAt(player.transform.position);
			if (Vector3.Distance(player.transform.position, this.transform.position) >= distanceToPlayerForDeath) {
				//Move towards the character..
				transform.position += transform.forward * speed * Time.deltaTime;
			} else {
				//Game over or injury..
				GameManager.SetGameOver(false);
			}
		}
	}

	float GenerateSpeed(int level)
	{
		//level 1 between 0.5 to 2
		if (level == 1) {
			return Random.Range(0.5f, 2f);
		}
		return Random.Range(2, 5);
	}
}
