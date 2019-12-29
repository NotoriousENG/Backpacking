using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BernieGangCourtEncounter : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerProgress playerProgress;
    PlayerObjectives playerObjectives;
    Objective objective;
    private GameObject obj;
    private GameObject obj2;
    private GameObject obj3;
    private GameObject bernieBlocker;
    private GameObject schoolBlock;
    private bool firstTime;
    private GameObject player;

  
   


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        bernieBlocker = GameObject.Find("bernieBlocker");
        obj = GameObject.Find("BernieGangMemberCourt(Middle)");
        obj2 = GameObject.Find("BernieGangMemberCourt(Left)");
        obj3 = GameObject.Find("BernieGangMemberCourt(Right)");
        firstTime = true;
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();
        playerObjectives = GameObject.Find("Player").GetComponent<PlayerObjectives>();
        string name = "Defeat Bernie's grunts";
        string description = "Bernie's grunts need to be dealt with before I can face Bernie.";
        objective = new Objective(name, description);
        schoolBlock = GameObject.Find("SchoolBlockade");

        playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0, 0);

        //if middle grunt has been beaten
        if (playerProgress.defeatedMiddleGrunt)
        {
            if (playerProgress.recentlyRan && player.transform.position.x < -51f && player.transform.position.x > -55f)
            {
                playerProgress.defeatedMiddleGrunt = false;
            }
            else
            {
                DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
                dialogueTrigger.ClearFields();
                dialogueTrigger.isAutoTalker = true;
                dialogueTrigger.dialogue.name = "Grunt";

                //if middle grunt was the last to be beaten
                if (playerProgress.defeatedRightGrunt && playerProgress.defeatedLeftGrunt)
                {
                    dialogueTrigger.dialogue.sentences = new string[2];
                    dialogueTrigger.dialogue.sentences[0] = "Wow, you're tougher than you look...";
                    dialogueTrigger.dialogue.sentences[1] = "[An objective was added to your phone]";
                    dialogueTrigger.createNewObjective = true;
                    dialogueTrigger.objectiveName = "Defeat Bernie";
                    dialogueTrigger.objectiveDescription = "Time to get my money back!";
                    obj.AddComponent<NPCMovement>();
                    obj.GetComponent<BoxCollider2D>().isTrigger = true;
                    dialogueTrigger.walksAway = true;
                    dialogueTrigger.destination = new Vector3(25.97f, 10f, -9.480469f);
                    Destroy(obj, 15f);
                }

                //middle grunt was not the last to be beaten
                else
                {
                    dialogueTrigger.dialogue.sentences = new string[1];
                    dialogueTrigger.dialogue.sentences[0] = "Wow, you're tougher than you look...";
                    obj.AddComponent<NPCMovement>();
                    obj.GetComponent<BoxCollider2D>().isTrigger = true;
                    dialogueTrigger.walksAway = true;
                    dialogueTrigger.destination = new Vector3(25.97f, 10f, -9.480469f);
                    Destroy(obj, 15f);
                }
            }
        }

        //if all grunts have been defeated, remove the blockade and let the player go face bernie
        if (playerProgress.defeatedLeftGrunt && playerProgress.defeatedMiddleGrunt && playerProgress.defeatedRightGrunt)
        {
            //remove gang encounter objective
            playerObjectives.phone.removeObjective(objective);

            //remove blockade
            schoolBlock.SetActive(false);
            bernieBlocker.SetActive(false);
        }

        //if left grunt was beaten
        if (playerProgress.defeatedLeftGrunt)
        {


                DialogueTrigger dialogueTrigger2 = obj2.GetComponent<DialogueTrigger>();

                dialogueTrigger2.ClearFields();
                dialogueTrigger2.isAutoTalker = true;
                dialogueTrigger2.dialogue.name = "Grunt";

                //if left grunt was last to be beaten
                if (playerProgress.defeatedRightGrunt && playerProgress.defeatedMiddleGrunt)
                {
                    dialogueTrigger2.dialogue.sentences = new string[2];
                    dialogueTrigger2.dialogue.sentences[0] = "Hey take it easy man..";
                    dialogueTrigger2.dialogue.sentences[1] = "[An objective was added to your phone]";
                    dialogueTrigger2.createNewObjective = true;
                    dialogueTrigger2.objectiveName = "Defeat Bernie";
                    dialogueTrigger2.objectiveDescription = "Time to get my money back!";
                    obj2.AddComponent<NPCMovement>();
                    obj2.GetComponent<BoxCollider2D>().isTrigger = true;
                    dialogueTrigger2.walksAway = true;
                    dialogueTrigger2.destination = new Vector3(60f, 10f, -9.480469f);
                    Destroy(obj2, 15f);
                }
                //left grunt was not last to be beaten
                else
                {
                    dialogueTrigger2.dialogue.sentences = new string[1];
                    dialogueTrigger2.dialogue.sentences[0] = "Hey take it easy man..";
                    obj2.AddComponent<NPCMovement>();
                    obj2.GetComponent<BoxCollider2D>().isTrigger = true;
                    dialogueTrigger2.walksAway = true;
                    dialogueTrigger2.destination = new Vector3(60f, 10f, -9.480469f);
                    Destroy(obj2, 15f);
                }
            
        }

        //if right grunt was beaten
        if (playerProgress.defeatedRightGrunt)
        {

           
                Debug.Log("Checking");
                DialogueTrigger dialogueTrigger3 = obj3.GetComponent<DialogueTrigger>();
                dialogueTrigger3.ClearFields();
                dialogueTrigger3.isAutoTalker = true;
                dialogueTrigger3.dialogue.name = "Grunt";

                //if right grunt was last to be beaten
                if (playerProgress.defeatedLeftGrunt && playerProgress.defeatedMiddleGrunt)
                {
                    Debug.Log("Checking");
                    dialogueTrigger3.dialogue.sentences = new string[2];
                    dialogueTrigger3.dialogue.sentences[0] = "..uh.. we were just messing with you *gulp*";
                    dialogueTrigger3.dialogue.sentences[1] = "[An objective was added to your phone]";
                    dialogueTrigger3.createNewObjective = true;
                    dialogueTrigger3.objectiveName = "Defeat Bernie";
                    dialogueTrigger3.objectiveDescription = "Time to get my money back!";
                    obj3.AddComponent<NPCMovement>();
                    obj3.GetComponent<BoxCollider2D>().isTrigger = true;
                    dialogueTrigger3.walksAway = true;
                    dialogueTrigger3.destination = new Vector3(-10f, 10f, -9.480469f);
                    Destroy(obj3, 15f);
                }
                //right grunt was not the last to be beaten
                else
                {
                    dialogueTrigger3.dialogue.sentences = new string[1];
                    dialogueTrigger3.dialogue.sentences[0] = "..uh.. we were just messing with you *gulp*";
                    obj3.AddComponent<NPCMovement>();
                    obj3.GetComponent<BoxCollider2D>().isTrigger = true;
                    dialogueTrigger3.walksAway = true;
                    dialogueTrigger3.destination = new Vector3(-10f, 10f, -9.480469f);
                    Destroy(obj3, 15f);
                }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (playerProgress.defeatedMiddleGrunt && firstTime)
        {
            //playerMovement.moveM.Play("Player_Idle_Down");
            playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0.5f, 0);
            //playerMovement.facing = "down";
            firstTime = false;
        }



    }

}
