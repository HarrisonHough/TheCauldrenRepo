using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [SerializeField]
    private CardboardInteract cardboard;
    private bool waitingForInput = false;
    public bool WaitingForInput { get { return waitingForInput; } }

	// Use this for initialization
	void Start () {
        if (cardboard == null)
            cardboard = FindObjectOfType<CardboardInteract>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();	
	}

    void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cardboard.Interact();
            waitingForInput = false;
            Debug.Log("Mouse Down Detected");
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Up Detected");
        }

        //run grab update
        cardboard.GrabUpdate();
    }

    public void StartWaitForInput()
    {
        waitingForInput = true;
    }
}
