using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEncounter : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerProgress playerProgress;
    private GameObject obj;
    private bool done;

    DialogueTrigger[] dialogueTriggers;
    NPCMovement[] npcMovements;
 
    private bool firstTime = true;
    private bool secondTime = false;
    // Start is called before the first frame update
    void Start()
    {
        done = false;
        
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();
        if (playerProgress.completedDogEncounter && !playerProgress.recentlyRan)
        {
            //Progress storyline
            obj = GameObject.Find("Dogs");
            DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
            dialogueTrigger.ClearFields();
            dialogueTrigger.isAutoTalker = true;
            dialogueTrigger.dialogue.name = "";
            dialogueTrigger.dialogue.sentences = new string[1];
            dialogueTrigger.dialogue.sentences[0] = "The dogs scurried off.";
            obj.AddComponent<NPCMovement>();
            dialogueTrigger.walksAway = true;
            dialogueTrigger.destination = new Vector3(25f, -5f, -9.480469f);
            SpriteRenderer[] spriteRenderers = GameObject.Find("Dogs").GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sprite = Resources.Load<Sprite>("Sprites/GREATdog");
            }
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");
        if (playerProgress.completedDogEncounter)
        {
            Debug.Log("Completed dog encounter");
            if (firstTime)
            {
                //playerMovement.moveM.Play("Player_Idle_Up");
                playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0, 0);
                //playerMovement.facing = "up";
                firstTime = false;
                secondTime = true;
            }
            else if (secondTime)
            {
                //playerMovement.moveM.Play("Player_Idle_Down");
                playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0.5f, 0);
                //playerMovement.facing = "down";
                secondTime = false;
            }
            //Make other grunts leave when main grunt leaves
            if (obj != null && playerMovement.enabled && obj.GetComponent<NPCMovement>() != null && playerMovement.enabled && obj.GetComponent<NPCMovement>().doneMoving == false)
            {
                obj.GetComponent<BoxCollider2D>().enabled = false;
                DialogueTrigger[] dialogueTriggers = obj.GetComponentsInChildren<DialogueTrigger>();
                for (int i = 0; i < dialogueTriggers.Length; i++)
                {
                    dialogueTriggers[i].walksAway = true;
                    dialogueTriggers[i].destination = new Vector3(25f, -5f, -9.480469f);
                    dialogueTriggers[i].transform.gameObject.AddComponent<NPCMovement>();
                    dialogueTriggers[i].transform.gameObject.GetComponent<NPCMovement>().moveNPC(dialogueTriggers[i].destination);
                }
                Destroy(obj, 5f);
                GameObject.Find("doggo_0").GetComponent<PatrollingEnemy>().enabled = true;
            }
        }
        else
        {
            Debug.Log("Haven't completed");
            DialogueManager dialogueManager = GameObject.Find("Text Box").GetComponent<DialogueManager>();
            if (dialogueManager.sentences.Count > 0)
            {
                Debug.Log("Sentences");
                if (dialogueManager.sentences.Peek() == "Here comes the attack!")
                {
                    Debug.Log("Transforming");
                    for (int i = 0; i < npcMovements.Length; i++)
                    {
                        npcMovements[i].moveNPC(dialogueTriggers[i].destination);
                    }
                }
            }
        }      
        
    }
}
