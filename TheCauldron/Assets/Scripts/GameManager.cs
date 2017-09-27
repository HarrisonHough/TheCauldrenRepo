using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>{

    //private static GameManager instance;
    //public static GameManager Instance { get { return instance; } }
    public static bool gameOver = false;
    private int maxWaves = 6;
    public static int wavesComplete = 0;
    public static bool wonLevel = false;

    [SerializeField]
    private GameObject room;

    [SerializeField]
    private float difficulty = 0f;
    [SerializeField]
    private float prevDifficulty = 0f;

    public EnemyManager EnemyManager;
    [SerializeField]
    private PaperMenu menu;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private IngredientsControl ingredientsControl;

    private bool waveInProgress = false;

    // Use this for initialization
    void Start()
    {
        if(EnemyManager == null)
            EnemyManager = FindObjectOfType<EnemyManager>();
        if (menu == null)
            menu = FindObjectOfType<PaperMenu>();
        if (playerInput == null)
            playerInput = FindObjectOfType<PlayerInput>();
        if(ingredientsControl == null)
            ingredientsControl = FindObjectOfType<IngredientsControl>();

        menu.ShowPaperMenu();
        menu.SetGetReadyPanel(true);
        //Wait for input
        StartCoroutine(WaitForNextWave());
    }

    public void NewWave()
    {
        //Start spawning
        this.EnemyManager.StartSpawningWave();
        waveInProgress = true;
    }

    public void SetGameOver(bool won)
    {
        gameOver = true;
        wonLevel = won;
        if (!won)
        {
            //Show lose / game over screen
            menu.ShowPaperMenu();
            menu.SetGameOverLosePanel(true);
            Debug.Log("Game over you lose");
            //start try again coroutine
            StartCoroutine(WaitForRetry());
        }
        else
        {
            //Show Win / Game over screen
            menu.ShowPaperMenu();
            menu.SetGameOverWinPanel(true);
            Debug.Log("Game Over you win");

        }
    }

    public void WaveComplete()
    {
        if (!waveInProgress)
        {
            Debug.Log("Something went wrong, wave complete should not have been called");
            return;
        }
        waveInProgress = false;
        wavesComplete++;
        // show wave complete

        Debug.Log("Waves Complete" + wavesComplete);

        if (wavesComplete >= maxWaves)
        {
            //game finished / won
            SetGameOver(true);
            return;
        }
        menu.ShowPaperMenu();
        menu.SetWaveCompletePanel(true);
        StartCoroutine(WaitForNextWave());
    }

    IEnumerator WaitForNextWave() {

        // Add back ingredients
        ingredientsControl.ResetIngredients();

        playerInput.StartWaitForInput();
        while (playerInput.WaitingForInput) {

            yield return null;
        }
        menu.HidePaperMenu();
        // start next wave
        NewWave();
    }

    IEnumerator WaitForRetry()
    {

        // Add back ingredients
        ingredientsControl.ResetIngredients();

        // hide / reset active ghosts
        EnemyManager.Reset();

        wavesComplete = 0;

        playerInput.StartWaitForInput();
        while (playerInput.WaitingForInput)
        {

            yield return null;
        }

        //reset gameover
        gameOver = false;
        menu.HidePaperMenu();
        // start next wave
        NewWave();
    }
}
