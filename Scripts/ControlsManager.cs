using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{

    public Animator animator;
    public bool isOpen;
    private PlayerMovement playerMovement;
    private talkWithButton talkwithButton;

    // Use this for initialization
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        talkwithButton = FindObjectOfType<talkWithButton>();
        if (playerMovement.justStartedGame == true)
        {
            Debug.Log("Just started");
            animator.SetBool("IsOpen", true);
            isOpen = true;
            playerMovement.justStartedGame = false;
        }
        else
        {
            Debug.Log("Returning to scene, skip controls");
            animator.SetBool("IsOpen", false);
            isOpen = false;
            playerMovement.enabled = true;
            talkwithButton.enabled = true;
        }
    }

    private void Update()
    {
        playerMovement.justStartedGame = false;
        if (isOpen)
        {
            playerMovement.enabled = false;
            talkwithButton.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Z) && isOpen)
        {
            animator.SetBool("IsOpen", false);
            isOpen = false;
            playerMovement.enabled = true;
            talkwithButton.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.X) && isOpen)
        {
            animator.SetBool("IsOpen", false);
            isOpen = false;
            playerMovement.enabled = true;
            talkwithButton.enabled = true;
        }
    }
}