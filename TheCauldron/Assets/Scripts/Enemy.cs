using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //should randomize the speed..
    //public float speed = 2f;
    // TODO remove this make spark function to check this
    public float distanceToPlayerForDeath = 1f;
    public GameObject player;
    public GameObject DeadEnemyPrefab;
    public GameObject LoseSound;

    public bool alive = true;
    public float speed;

    private AudioSource audioSource;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        //set inactive as default
        SetInactive();
    }

    public void SetInactive()
    {
        alive = false;
        transform.position = new Vector3(0,-10,0);
        GameManager.Instance.EnemyManager.UpdateClosestEnemy();
        audioSource.Stop();
    }

    public void Restart()
    {
        alive = true;
        speed = GenerateSpeed(GameManager.wavesComplete);
        audioSource.Play();
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {

        // Look at player
        transform.LookAt(player.transform.position);
        while (alive)
        {
            if (!GameManager.gameOver)
            {
                
                if (Vector3.Distance(player.transform.position, this.transform.position) >= distanceToPlayerForDeath)
                {
                    //Move towards the character..
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                else
                {
                    //Enemy got you Game over
                    GameManager.Instance.SetGameOver(false);
                    SetInactive();
                }
            }
            yield return null;
        }

    }

    public void OnHit()
    {
        GameManager.Instance.EnemyManager.EnemyKilled(transform.position, transform.rotation);

        SetInactive();
    }


    private float GenerateSpeed(int level)
    {
        //level 1 between 0.5 to 2
        if (level == 1)
        {
            return Random.Range(0.5f, 2f);
        }
        return Random.Range(2, 5);
    }
}
