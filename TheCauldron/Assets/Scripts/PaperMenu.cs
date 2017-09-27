using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperMenu : MonoBehaviour {

    [SerializeField]
    private GameObject paperMenuMesh;
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private GameObject audioPanel;
    [SerializeField]
    private GameObject waveCompletePanel;
    [SerializeField]
    private GameObject gameOverLosePanel;
    [SerializeField]
    private GameObject gameOverWinPanel;
    [SerializeField]
    private GameObject introPanel;
    [SerializeField]
    private GameObject tutorialPanel;
    [SerializeField]
    private GameObject getReadyPanel;
    [SerializeField]
    private GameObject resultsPanel;
    [SerializeField]
    private Text wavesCompletedText;
    [SerializeField]
    private Text enemiesKilledText;


    // Use this for initialization
    void Awake () {

        //hide all panels
        HideAllPanels();
        HidePaperMenu();
	}

    public void HidePaperMenu()
    {
        paperMenuMesh.SetActive(false);
        HideAllPanels();
    }

    public void ShowPaperMenu()
    {
        paperMenuMesh.SetActive(true);
    }

    public void HideAllPanels()
    {
        SetOptionsPanel(false);
        SetAudioPanel(false);
        SetGameOverLosePanel(false);
        SetGameOverWinPanel(false);
        SetWaveCompletePanel(false);
        SetIntroPanel(false);
        SetTutorialPanel(false);
        SetGetReadyPanel(false);

    }

    public void SetOptionsPanel (bool enabled)
    {
        optionsPanel.SetActive(enabled);
    }

    public void SetAudioPanel(bool enabled)
    {
        audioPanel.SetActive(enabled);
    }

    public void SetWaveCompletePanel(bool enabled)
    {
        waveCompletePanel.SetActive(enabled);
        // Wave complete show results
        SetResultsPanel(enabled);
    }

    public void SetGameOverLosePanel(bool enabled)
    {
        gameOverLosePanel.SetActive(enabled);
        // game over show results
        SetResultsPanel(enabled);
    }

    public void SetGameOverWinPanel(bool enabled)
    {
        gameOverWinPanel.SetActive(enabled);
        // game over show results
        SetResultsPanel(enabled);
    }

    public void SetIntroPanel(bool enabled)
    {
        introPanel.SetActive(enabled);
    }

    public void SetTutorialPanel(bool enabled)
    {
        introPanel.SetActive(enabled);
    }

    public void SetGetReadyPanel(bool enabled)
    {
        getReadyPanel.SetActive(enabled);
    }

    public void SetResultsPanel(bool enabled)
    {
        resultsPanel.SetActive(enabled);
        wavesCompletedText.text = "Waves Completed \n" + GameManager.wavesComplete;
        enemiesKilledText.text = "Enemies Killed \n" + GameManager.Instance.EnemyManager.TotalEnemiesKilled;

    }




}
