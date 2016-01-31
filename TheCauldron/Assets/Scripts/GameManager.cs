using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static bool gameOver = false;
	public static int level = 1;
	public static bool wonLevel = false;
	public GameObject room;
	// sound effects: public GameObject ;
	public float difficulty = 0f;

	void Start() {
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
	}

}
