using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardboardInteract : MonoBehaviour {


    public ParticleSystem SelectedItemParticles;
    public GameObject musicItemButton;
    public Text musicItemText;
    public GameObject soundEffectsItemButton;
    public Text soundEffectsItemText;
    public GameObject creditsItemButton;
    public Text creditsItemText;
    public Text gameOverText;
    public Text enemiesKilledText;
    public Text waveText;
    
    //public InventoryLoader inventoryLoader;
    //public EnemyLoader[] enemyLoader;

    GameObject[] items;
    bool holdingItem;
    GameObject currentItemHeld;
    float timeInteracted;
    float itemHoldDistance = 1.0f;
    float smooth = 4;
    bool waitingForNextWave = false;
    float timeToWaitForNextWave = 3f;
    float timeWon;

    // Use this for initialization
    void Start () {
        items = GameObject.FindGameObjectsWithTag("Item");
    }

    public void Interact()
    {
        //if ((VRDevice.family != "oculus" && Cardboard.SDK.Triggered) || Input.GetMouseButtonUp(0))
        
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (!holdingItem && Time.time > timeInteracted + 0.3f)
            {
                CheckIngredientHit(hit);                
            }

            if (holdingItem && Time.time > timeInteracted + 0.3f)
            {
                StopHoldingIngredient();
            }
        }
        
    }

    private void CheckIngredientHit(RaycastHit hit)
    {
        if (hit.collider.gameObject.tag == "Item")
        {
            holdingItem = true;
            currentItemHeld = hit.collider.gameObject;
            timeInteracted = Time.time;
        }

    }

    private void StopHoldingIngredient()
    {
        holdingItem = false;
        currentItemHeld.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        // SelectedItemParticles.transform.position = new Vector3(0, -10, 0);
        currentItemHeld = null;
        timeInteracted = Time.time;
    }

    public void GrabUpdate()
    {
        if (holdingItem && currentItemHeld != null)
        {
            Vector3 frontOfCamera = Camera.main.transform.position + Camera.main.transform.forward * itemHoldDistance;
            Vector3 diffVector = frontOfCamera - currentItemHeld.transform.position;
            float diff = diffVector.magnitude;
            currentItemHeld.transform.position = Vector3.Lerp(currentItemHeld.transform.position, frontOfCamera, Time.deltaTime * smooth);
            // SelectedItemParticles.transform.position = itemHeld.transform.position;
            currentItemHeld.transform.rotation *= Quaternion.Euler(Random.Range(5, 10) * diff * (diffVector.z / Mathf.Abs(diffVector.z)), 0, 0);
        }
    }

    private void DisplayGameOver()
    {
        //play death sound
        SoundManager.Instance.PlayRandomWitchDeath();
        //display canvas
        //update score and waves complete

        //TODO check if this needed
        GameManager.gameOver = false;

    }
}
