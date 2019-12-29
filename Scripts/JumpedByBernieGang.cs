using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpedByBernieGang : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerProgress playerProgress;
    private GameObject _obj;
    private bool done;
    //basketball court destination
    Vector3 court;
 
    private bool firstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        done = false;
        
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();
        if (playerProgress.completedFirstBernieGangEncounter && playerProgress.recentlyWon)
        {
            //Progress storyline
            GameObject obj = GameObject.Find("Bernie Gang Member (Middle)");
            DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
            dialogueTrigger.ClearFields();
            dialogueTrigger.isAutoTalker = true;
            dialogueTrigger.dialogue.name = "Grunt";
            dialogueTrigger.dialogue.sentences = new string[7];
            dialogueTrigger.dialogue.sentences[0] = "Haha! Those 10 bucks are ours!";
            dialogueTrigger.dialogue.sentences[1] = "Let's go, boys!";
            dialogueTrigger.dialogue.sentences[2] = "We'll meet up with everyone back at HQ.";
            dialogueTrigger.dialogue.sentences[3] = "What was that, kid? Where's our headquarters?";
            dialogueTrigger.dialogue.sentences[4] = "It's at the basketball court.";
            dialogueTrigger.dialogue.sentences[5] = "I'm only telling you because you don't stand a chance!";
            dialogueTrigger.dialogue.sentences[6] = "[An objective was added to your phone]";
            obj.AddComponent<NPCMovement>();
            dialogueTrigger.walksAway = true;
            dialogueTrigger.destination = new Vector3(40f, -7f, -9.480469f);
            dialogueTrigger.createNewObjective = true;
            dialogueTrigger.objectiveName = "Find Bernie Gang HQ";
            dialogueTrigger.objectiveDescription = "The Bernie Gang stole my money! Apparently their HQ is at the basketball court.";

            //playerMovement.moveM.Play("Player_Idle_Up");
            playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0.5f, 0);
            //playerMovement.facing = "up";



        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject obj = GameObject.Find("Bernie Gang Member (Middle)");


            if (playerProgress.completedFirstBernieGangEncounter && firstTime)
            {
                //playerMovement.moveM.Play("Player_Idle_Down");
                playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0, 0);
                //playerMovement.facing = "down";
                firstTime = false;
            }
            //Make other grunts leave when main grunt leaves
           if (obj != null && playerProgress.completedFirstBernieGangEncounter && GameObject.Find("Bernie Gang Member (Middle)").GetComponent<NPCMovement>() != null && GameObject.Find("Bernie Gang Member (Middle)").GetComponent<NPCMovement>().doneMoving == false)
            {
                GameObject leftGrunt = GameObject.Find("Bernie Gang Member (Left)");
                DialogueTrigger leftDialogueTrigger = leftGrunt.GetComponent<DialogueTrigger>();
                leftDialogueTrigger.walksAway = true;
                leftDialogueTrigger.destination = new Vector3(40f, -7f, -9.480469f);

                leftGrunt.AddComponent<NPCMovement>();
                leftGrunt.GetComponent<NPCMovement>().moveNPC(leftDialogueTrigger.destination);

                GameObject rightGrunt = GameObject.Find("Bernie Gang Member (Right)");
                DialogueTrigger rightDialogueTrigger = rightGrunt.GetComponent<DialogueTrigger>();
                rightDialogueTrigger.walksAway = true;
                rightDialogueTrigger.destination = new Vector3(40f, -7f, -9.480469f);

                rightGrunt.AddComponent<NPCMovement>();
                rightGrunt.GetComponent<NPCMovement>().moveNPC(rightDialogueTrigger.destination);
                Destroy(leftGrunt, 5f);
                Destroy(rightGrunt, 5f);
                Destroy(obj, 5f);

            obj.GetComponent<Animator>().SetBool("isWalking", true);
            obj = GameObject.Find("Bernie Gang Member (Right)");
            obj.GetComponent<Animator>().SetBool("isWalking", true);
            obj = GameObject.Find("Bernie Gang Member (Left)");
            obj.GetComponent<Animator>().SetBool("isWalking", true);


        }
           
        
    }
}
