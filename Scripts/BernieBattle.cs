using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BernieBattle : MonoBehaviour
{

    PlayerMovement playerMovement;
    PlayerProgress playerProgress;
    private GameObject bernie;
    private GameObject money;
    private bool firstTime;
   
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();
        bernie = GameObject.Find("Bernie");
        money = GameObject.Find("playerMoney");
        firstTime = true;

        playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0, 0);

        if (playerProgress.defeatedBernie && playerProgress.recentlyWon)
        {
            money.GetComponent<SpriteRenderer>().enabled = true;
            money.GetComponent<BoxCollider2D>().enabled = true;
            bernie.GetComponent<BoxCollider2D>().isTrigger = true;
            DialogueTrigger dialogueTrigger = bernie.GetComponent<DialogueTrigger>();
            dialogueTrigger.ClearFields();
            dialogueTrigger.isAutoTalker = true;
            dialogueTrigger.dialogue.name = "Bernie";

            dialogueTrigger.dialogue.sentences = new string[9];
            dialogueTrigger.dialogue.sentences[0] = "You think you've won?";
            dialogueTrigger.dialogue.sentences[1] = "I can never be defeated.";
            dialogueTrigger.dialogue.sentences[2] = "I will be back stronger than ever,";
            dialogueTrigger.dialogue.sentences[3] = "and next time I won't hold back!";
            dialogueTrigger.dialogue.name = "";
            dialogueTrigger.dialogue.sentences[4] = "[Bernie goes to run away but trips and falls]";
            dialogueTrigger.dialogue.sentences[5] = "[Ha! what a loser]";
            dialogueTrigger.dialogue.sentences[6] = "[As he gets back up and runs off, you notice...]";
            dialogueTrigger.dialogue.sentences[7] = "[Your money is on the ground!]";
            dialogueTrigger.dialogue.sentences[8] = "[You got your 10 dollars back]";
            bernie.AddComponent<NPCMovement>();
            dialogueTrigger.walksAway = true;
            dialogueTrigger.destination = new Vector3(0f, 22.92f, -13.41797f);
            Destroy(bernie, 20f);
        }
     
    }

    // Update is called once per frame
    void Update()
    {
        if (playerProgress.defeatedBernie && firstTime == true)
        {
            playerMovement.dialogueCollider.transform.localPosition = new Vector3(-1, 0.5f, 0);

            money.GetComponent<SpriteRenderer>().enabled = true;
            money.GetComponent<BoxCollider2D>().enabled = true;


            //obj2.SetActive(true);
            //obj3.SetActive(true);

            firstTime = false;
        }
      
    }
}
