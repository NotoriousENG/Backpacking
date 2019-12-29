using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatsRunAway : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerProgress playerProgress;
    private GameObject doggo;
    private bool done;

    DialogueTrigger[] dialogueTriggers;
    NPCMovement[] npcMovements;
    DialogueTrigger dialogueTrigger;
    NPCMovement npcMovement;


    private bool firstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        done = false;

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();
        doggo = GameObject.Find("doggo_0");
        /*if (playerProgress.completedDogEncounter && doggo.transform.localPosition.x > 29.5f && doggo.transform.localPosition.y < -4.5f && doggo.transform.localPosition.y > 7.0f)
        {
            //Progress storyline
            obj = GameObject.Find("Cats");
            DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
            dialogueTrigger.ClearFields();
            dialogueTrigger.isAutoTalker = true;
            dialogueTrigger.dialogue.name = "";
            dialogueTrigger.dialogue.sentences = new string[1];
            dialogueTrigger.dialogue.sentences[0] = "The dogs scurried off.";
            obj.AddComponent<NPCMovement>();
            dialogueTrigger.walksAway = true;
            dialogueTrigger.destination = new Vector3(25f, -5f, -9.480469f);

            //playerMovement.moveM.Play("Player_Idle_Up");
            playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0, 0);
            //playerMovement.facing = "up";
        }
        else
        {
            dialogueTriggers = GameObject.Find("Dogs").GetComponentsInChildren<DialogueTrigger>();
            npcMovements = GameObject.Find("Dogs").GetComponentsInChildren<NPCMovement>();
            for (int i = 0; i < dialogueTriggers.Length; i++)
            {
                dialogueTriggers[i].walksAway = true;
                dialogueTriggers[i].destination = new Vector3(0f, 0.25f, 0f);
            }
        }*/
        if (playerProgress.completedDogEncounter)
        {
            dialogueTrigger = GameObject.Find("Cats").GetComponent<DialogueTrigger>();
            dialogueTrigger.ClearFields();
            dialogueTrigger.dialogue.name = "";
            dialogueTrigger.dialogue.sentences = new string[1];
            dialogueTrigger.dialogue.sentences[0] = "The cats ran away!";
            //dialogueTrigger.dialogue.sentences[1] = "...and so did the old lady!";
            dialogueTrigger.removableObject = true;
            dialogueTrigger.isAutoTalker = true;
            dialogueTriggers = GameObject.Find("Cats").GetComponentsInChildren<DialogueTrigger>();
            npcMovements = GameObject.Find("Cats").GetComponentsInChildren<NPCMovement>();
            BoxCollider2D boxCollider2D = GameObject.Find("Cats").GetComponent<BoxCollider2D>();
            boxCollider2D.offset = new Vector2(-47.8f, -105.3f);

            //npcMovement = GameObject.Find("Cat Feeder").GetComponent<NPCMovement>();
            //dialogueTrigger = GameObject.Find("Cat Feeder").GetComponent<DialogueTrigger>();
            //dialogueTrigger.ClearFields();
            //dialogueTrigger.dialogue.name = "Lady Formerly Feeding Cats";
            //dialogueTrigger.dialogue.sentences = new string[1];
            //dialogueTrigger.dialogue.sentences[0] = "Oh kitties, where have you gone?";
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");
        if (playerProgress.completedDogEncounter && doggo.transform.localPosition.x > 27.5f && doggo.transform.localPosition.y < -4.5f && doggo.transform.localPosition.y > -7.0f)
        {
            Debug.Log("Length: " + npcMovements.Length);
            for (int i = 0; i < npcMovements.Length; i++)
            {
                if (npcMovements[i] != null && dialogueTriggers[i] != null)
                {
                    npcMovements[i].moveNPC(dialogueTriggers[i+1].destination);
                    //npcMovement.moveNPC(dialogueTriggers[i.destination);
                    Destroy(dialogueTriggers[i+1].gameObject, 5f);
                }
            }
        }
    }
}
