using UnityEngine;
using System.Collections;

public class DeadEnemy : MonoBehaviour {

  public float SecondsToDisappear;
  public Material DeadEnemyMaterial;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
    SecondsToDisappear -= Time.deltaTime;
    if (SecondsToDisappear < 0) {
      Destroy(gameObject);
    } else {
      DeadEnemyMaterial.SetColor("_TintColor", new Color(255, 255, 255, SecondsToDisappear * 0.25f));
    }
	}
}
