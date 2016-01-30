using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static bool gameOver;
	public static int level;
	public static bool wonLevel;

	void Start() {
		gameOver = false;
		level = 1;
		wonLevel = false;
	}

	public static void SetGameOver(bool won) {
		gameOver = true;
		wonLevel = won;
	}

}
