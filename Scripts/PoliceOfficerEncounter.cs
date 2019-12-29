using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceOfficerEncounter : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerProgress playerProgress;
    private GameObject obj;
    private bool done;
    //basketball court destination
    Vector3 court;

    private bool firstTime = true;
    private bool firstTimeAgain = true;
    private bool secondTime = false;
    private bool thirdTime = false;
    // Start is called before the first frame update
    void Start()
    {
        done = false;

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerProgress.donutAcquired == true && playerProgress.defeatedPoliceOfficer == false && firstTime)
        {
            //Progress storyline
            obj = GameObject.Find("Police Officer");
            DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
            dialogueTrigger.isAutoTalker = true;
            dialogueTrigger.dialogue.sentences = new string[4];
            dialogueTrigger.dialogue.sentences[0] = "Huh, you brought me a donut, you say?";
            dialogueTrigger.dialogue.sentences[1] = "What the hell kinda donut is this?";
            dialogueTrigger.dialogue.sentences[2] = "I'm sick of you kids and your pranks.";
            dialogueTrigger.dialogue.sentences[3] = "It's one trip to the slammer for you, boy!";
            dialogueTrigger.playerProgressBoolToMakeTrue = "defeatedPoliceOfficer";
            dialogueTrigger.isSceneChanger = true;
            dialogueTrigger.sceneName = "Battle";
            dialogueTrigger.battleBackground = Resources.Load<Sprite>("Sprites/Battle Backgrounds/neighborhood");
            dialogueTrigger.createNewObjective = false;
            dialogueTrigger.walksAway = false;

            //playerMovement.moveM.Play("Player_Idle_Up");
            //playerMovement.facing = "up";
            
            firstTime = false;
        }
        if (playerProgress.defeatedPoliceOfficer == true && firstTimeAgain && playerProgress.recentlyWon)
        {
            obj = GameObject.Find("Police Officer");
            DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();

            dialogueTrigger.ClearFields();
            dialogueTrigger.isAutoTalker = true;
            dialogueTrigger.dialogue.name = "Police Officer";
            dialogueTrigger.dialogue.sentences = new string[1];
            dialogueTrigger.dialogue.sentences[0] = "Jeez, kid, I just wanted a donut!";
            dialogueTrigger.walksAway = true;
            dialogueTrigger.destination = new Vector3(-78.49f, -35f, -9.480469f);
            //playerMovement.moveM.Play("Player_Idle_Up");
            playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0.5f, 0);
            //playerMovement.facing = "up";

            firstTimeAgain = false;
            secondTime = true;
        }
        else if (playerProgress.defeatedPoliceOfficer == true && secondTime)
        {
            //playerMovement.moveM.Play("Player_Idle_Down");
            playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0, 0);
            //playerMovement.facing = "down";
            secondTime = false;
            thirdTime = true;
        }
        else if (playerProgress.defeatedPoliceOfficer == true && thirdTime)
        {
            obj.GetComponent<BoxCollider2D>().enabled = false;
            thirdTime = false;
        }

        /*
        //Make other grunts leave when main grunt leaves
        if (obj != null && playerProgress.donutAcquired && GameObject.Find("Police Officer").GetComponent<NPCMovement>() != null && GameObject.Find("Police Officer").GetComponent<NPCMovement>().doneMoving == false)
        {
            //Destroy(obj, 5f);
            DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
            dialogueTrigger.isAutoTalker = false;
            dialogueTrigger.dialogue.sentences = new string[1];
            dialogueTrigger.dialogue.sentences[0] = "Jeez, kid, I just wanted a donut!";
            dialogueTrigger.isSceneChanger = false;
            dialogueTrigger.sceneName = "";
            dialogueTrigger.createNewObjective = false;

        }
        //Police officer is in position after battle
        if (obj != null && obj.transform.localPosition == new Vector3(27.41f, -3.65f, -9.480469f))
        {
            DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
            dialogueTrigger.walksAway = false;
        }*/

    }
}
