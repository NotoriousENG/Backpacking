using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEncounter : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerProgress playerProgress;
    PlayerObjectives playerObjectives;
    Objective objective1;
    Objective objective2;
    private GameObject trigger;
    private GameObject blockade;
    private GameObject obj;
    private GameObject obj2;
    private GameObject obj3;
    private bool firstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();
        playerObjectives = GameObject.Find("Player").GetComponent<PlayerObjectives>();
        obj = GameObject.Find("KnightLeader");
        obj2 = GameObject.Find("Knight1");
        obj3 = GameObject.Find("Knight2");
        blockade = GameObject.Find("KnightBlockade");
        trigger = GameObject.Find("KnightTrigger");
        string name1 = "Pick up the sword";
        string description1 = "I saw a sword stuck in some stone. I should try to pick it up";
        string name2 = "Fight the Knight Leader";
        string description2 = "The knight wants me to test my new sword out in battle";
        objective1 = new Objective(name1, description1);
        objective2 = new Objective(name2, description2);


        if (playerProgress.pickedUpSword)
        {
            playerObjectives.phone.removeObjective(objective1);
        }
        if (playerProgress.defeatedKnight && playerProgress.recentlyWon)
        {
            playerObjectives.phone.removeObjective(objective2);
            Destroy(blockade);
            DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
            dialogueTrigger.ClearFields();
            dialogueTrigger.isAutoTalker = true;
            dialogueTrigger.dialogue.name = "Knight Leader";
            dialogueTrigger.dialogue.sentences = new string[2];
            dialogueTrigger.dialogue.sentences[0] = "Wow... you really are the champion";
            dialogueTrigger.dialogue.sentences[1] = "Go forth and bring peace to the land!";
            obj.AddComponent<NPCMovement>();
            dialogueTrigger.walksAway = true;
            dialogueTrigger.destination = new Vector3(50.94f, -20f, -9.480469f);
            Destroy(obj, 10f);
          
        }

    }

    // Update is called once per frame
    void Update()
    {
       
        if (playerProgress.pickedUpSword && firstTime == true)
        {
            blockade.GetComponent<BoxCollider2D>().enabled = true;
            obj.GetComponent<SpriteRenderer>().enabled = true;
            obj.GetComponent<BoxCollider2D>().enabled = true;
           

            trigger.GetComponent<BoxCollider2D>().enabled = true;


            //obj2.SetActive(true);
            //obj3.SetActive(true);

            firstTime = false;
        }

    }
}
