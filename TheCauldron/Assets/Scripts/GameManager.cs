using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static bool gameOver = false;
	public static int level = 1;
	public static bool wonLevel = false;
	public GameObject room;
	// sound effects: public GameObject ;

	void Start() {
	}

	public static void SetGameOver(bool won) {
		gameOver = true;
		wonLevel = won;
	}

}
