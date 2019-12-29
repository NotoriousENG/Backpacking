using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkWithButton : MonoBehaviour
{
    public bool inFrontOfNPC;
    public bool isTalking;
    //public DialogueTrigger thisTrigger;
    public DialogueManager thisManager;
    public PlayerMovement thisMovement;
    public bool inCollider;
    private GameObject currentObj;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!thisManager.isOpen)
        {
            thisMovement.enabled = true;
            inFrontOfNPC = false;
            isTalking = false;
            if (currentObj != null && currentObj.GetComponent<NPCMovement>() != null)
            {
                currentObj.GetComponent<NPCMovement>().enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) && isTalking)
        {
            thisManager.DisplayNextSentence();
        }
        else if (Input.GetKeyDown(KeyCode.Z) && currentObj != null)
        {
            Debug.Log("Talk");
            TalkToMe();
            isTalking = true;


        }
        else if (currentObj != null && isTalking == false && currentObj.tag == "Popup")
        {
            Debug.Log("Popup");
            TalkToMe();
            isTalking = true;
            //currentObj = null;
        }
        else if (currentObj != null && isTalking == false && thisMovement.doneMoving && currentObj.GetComponent<DialogueTrigger>().isAutoTalker)
        {
            Debug.Log("Auto Talker");
            TalkToMe();
            isTalking = true;
            currentObj = null;
        }
        if (currentObj != null)
        {
            //Debug.Log(currentObj.tag);
        }

    }

    void TalkToMe()
    {
        if (currentObj != null)
        {
            currentObj.GetComponent<DialogueTrigger>().TriggerDialogue();
            thisMovement.moveM.SetFloat("MoveX", 0);
            thisMovement.moveM.SetFloat("MoveY", 0);
            if (thisMovement.facing == "left")
            {
                thisMovement.moveM.Play("Player_Idle_Left");
                thisMovement.moveM.SetBool("isLeft", false);
            }
            if (thisMovement.facing == "right")
            {
                thisMovement.moveM.Play("Player_Idle_Right");
                thisMovement.moveM.SetBool("isRight", false);
            }
            if (thisMovement.facing == "up")
            {
                thisMovement.moveM.Play("Player_Idle_Up");
                thisMovement.moveM.SetBool("isUp", false);
            }
            if (thisMovement.facing == "down")
            {
                thisMovement.moveM.Play("Player_Idle_Down");
                thisMovement.moveM.SetBool("isDown", false);
            }
            thisMovement.moveM.SetBool("isWalking", false);
            thisMovement.enabled = false;
        }
        if (currentObj != null && currentObj.GetComponent<NPCMovement>() != null && currentObj.GetComponent<NPCMovement>().randomMovement == true)
        {
            currentObj.GetComponent<NPCMovement>().enabled = false;
        }
    }

    //When the player enters npc/object trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        //Debug.Log(collision.tag);
        //if the "object" is an npc
        if (collision.CompareTag("NPC") || collision.CompareTag("Interactable") || collision.CompareTag("Popup"))
        {
            currentObj = collision.gameObject;
            Debug.Log("Collision name: " + currentObj.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Collision");

        if (collision.CompareTag("NPC") || collision.CompareTag("Interactable") || collision.CompareTag("Popup"))
        {
            if (collision.gameObject == currentObj)
            {
                //currentObj.GetComponent<DialogueTrigger>().enabled = false;
                currentObj = null;
            }

        }

    }
}
