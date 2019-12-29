using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;

    public Animator animator;
    public bool isOpen;

    private bool isWriting;

    private bool isSceneChanger;
    private string sceneName;
    private string battleName;
    public Dialogue lowHealthBattleDialogue;
    public Dialogue lowmidHealthBattleDialogue;
    public Dialogue midhighHealthBattleDialogue;
    public Dialogue highHealthBattleDialogue;
    private Sprite battleSprite;
    private int battleMaxHealth;
    private string[] battleWeaknesses;
    private int battlePower;
    private float battleAccuracy;

    private bool isItemGiver;
    private bool isItemConsumer;
    private bool isItemChecker;
    private bool isRemovable;
    private bool playerHasItem;
    private string itemName;
    private string itemDescription;
    private Sprite itemSprite;
    private string itemCategory;
    private Attack[] attacks;
    private bool walksAway;
    private Vector3 destination;
    private string direction;
    private int offset;
    private GameObject obj;
    private Sprite battleBackground;

    private bool createNewObjective;
    private string objectiveName;
    private string objectiveDescription;

    private talkWithButton talkwithButton;
    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;
    private PlayerObjectives playerObjectives;

    private string playerProgressBoolToMakeTrue;
    


   private bool fadingOut;

    private bool walksAwayAfterConsuming;
    
    public Queue<string> sentences;

    // Use this for initialization
    void Start()
    {

        sentences = new Queue<string>();
        isSceneChanger = false;
        sceneName = "";
        isItemGiver = false;
        createNewObjective = false;
        isRemovable = false;
        isItemConsumer = false;
        isItemChecker = false;
        walksAway = false;
        destination = new Vector3();
        obj = null;
        offset = 0;
        itemName = "";
        itemDescription = "";
        itemSprite = null;
        itemCategory = "";
        attacks = null;
        direction = "";
        playerProgressBoolToMakeTrue = "";
      

        talkwithButton = FindObjectOfType<talkWithButton>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerObjectives = FindObjectOfType<PlayerObjectives>();

        talkwithButton.thisManager = FindObjectOfType<DialogueManager>();
    }

 

    public void StartDialogue(Dialogue dialogue)
    {
        if (isItemChecker)
        {

            Item item = new Item(itemName, itemDescription, itemSprite);
            string s = itemName;
            playerHasItem = playerInventory.backpack.hasItem(s);

            if (playerHasItem)
            {
                Debug.Log("Destroy because player has item");
                Destroy(obj);
                return;
            }
        }
        animator.SetBool("IsOpen", true);
        isOpen = true;

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        if (isItemGiver)
        {
            //sentences.Enqueue("You got a " + itemName + "!");
        }

        if (isItemConsumer)
        {

            Item item = new Item(itemName);
            string s = itemName;
            playerHasItem = playerInventory.backpack.hasItem(s);

            walksAwayAfterConsuming = walksAway;

            if (playerHasItem)
            {
                //sentences.Enqueue("You used the " + itemName + "!");
                playerInventory.backpack.removeItem(item);
                if (walksAwayAfterConsuming)
                {
                    walksAway = true;
                }

            }
            else
            {
                if (walksAwayAfterConsuming)
                {
                    walksAway = false;
                }
            }
        }
        if (isSceneChanger)
        {
            //sentences.Enqueue("The washing machine attacked!");
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            if(isRemovable)
            {
                Debug.Log("Remove object");
                Destroy(obj);
            }
            EndDialogue();
            return;
        }
        if (!isWriting)
        {
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
           
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isWriting = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        isWriting = false;
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        isOpen = false;
        if (obj.CompareTag("Popup"))
        {
            playerMovement.enabled = true;
            Vector3 playerPosition = playerMovement.transform.position;
           
            Vector3 newPosition = new Vector3(playerPosition.x, playerPosition.y + offset, playerPosition.z);
            playerMovement.movePlayer(newPosition);
        }
        if (createNewObjective)
        {
            Objective newObjective = new Objective(objectiveName, objectiveDescription);
            playerObjectives.phone.addObjective(newObjective);
        }
        if (isItemGiver)
        {
            if (itemCategory == "Weapon")
            {
                List<Attack> attackList = new List<Attack>();
                for (int i = 0; i < attacks.Length; i++)
                {
                    attackList.Add(attacks[i]);
                }
                Weapon weapon = new Weapon(itemName, itemDescription, itemSprite, attackList);
                playerInventory.backpack.addItem(weapon);
                if (playerInventory.backpack.getEquippedWeapon() == null)
                {
                    playerInventory.backpack.setEquippedWeapon(weapon);
                }
            }
            else
            {
                //Give item to player
                Item item = new Item(itemName, itemDescription, itemSprite);
                playerInventory.backpack.addItem(item);
            }
        }
        //If talking to NPC with a storyline trigger, set the trigger to true
        //Allows for scripted storyline events to take place
        PlayerProgress playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();
        if (playerProgressBoolToMakeTrue == "completedFirstBernieGangEncounter")
        {
            playerProgress.completedFirstBernieGangEncounter = true;
        }
        if (playerProgressBoolToMakeTrue == "completedDogEncounter")
        {
            playerProgress.completedDogEncounter = true;
        }
        if (playerProgressBoolToMakeTrue == "defeatedMiddleGrunt")
        {
            playerProgress.defeatedMiddleGrunt = true;
        }
        if (playerProgressBoolToMakeTrue == "defeatedLeftGrunt")
        {
            playerProgress.defeatedLeftGrunt = true;
        }
        if (playerProgressBoolToMakeTrue == "defeatedRightGrunt")
        {
            playerProgress.defeatedRightGrunt = true;
        }
        if (playerProgressBoolToMakeTrue == "donutAcquired")
        {
            playerProgress.donutAcquired = true;
        }
        if (playerProgressBoolToMakeTrue == "pickedUpSword")
        {
            playerProgress.pickedUpSword = true;
        }
        if (playerProgressBoolToMakeTrue == "washerDefeated")
        {
            playerProgress.washerDefeated = true;
        }
        if (playerProgressBoolToMakeTrue == "defeatedPoliceOfficer")
        {
            playerProgress.defeatedPoliceOfficer = true;
        }
        if (playerProgressBoolToMakeTrue == "defeatedKnight")
        {
            playerProgress.defeatedKnight = true;
        }
        if (playerProgressBoolToMakeTrue == "defeatedBernie")
        {
            playerProgress.defeatedBernie = true;
        }
        if (playerProgressBoolToMakeTrue == "gotMoney")
        {
            playerProgress.gotMoney = true;
        }

        if (isSceneChanger)
        {
            //Transfer the enemy to the battle scene
            GameObject objectToTransfer = (GameObject)Instantiate(Resources.Load("Prefabs/Enemy"));
            BattleEnemy battleEnemy = objectToTransfer.GetComponent<BattleEnemy>();
            battleEnemy.name = battleName;
            battleEnemy.image.sprite = battleSprite;
            battleEnemy.maxHealth = battleMaxHealth;
            battleEnemy.lowHealthBattleDialogue = lowHealthBattleDialogue;
            battleEnemy.lowmidHealthBattleDialogue = lowmidHealthBattleDialogue;
            battleEnemy.midhighHealthBattleDialogue = midhighHealthBattleDialogue;
            battleEnemy.highHealthBattleDialogue = highHealthBattleDialogue;

            battleEnemy.weaknesses = battleWeaknesses;
            battleEnemy.power = battlePower;
            battleEnemy.accuracy = battleAccuracy;
            objectToTransfer.AddComponent<MoveObjectBetweenScenes>();

            GameObject battleBackgroundToTransfer = (GameObject)Instantiate(Resources.Load("Prefabs/Background"));
            Image image = battleBackgroundToTransfer.GetComponent<Image>();
            image.sprite = battleBackground;

            battleBackgroundToTransfer.AddComponent<MoveObjectBetweenScenes>();

            //Load new scene
            SceneManager.LoadScene(sceneName);
        }
        if (walksAway)
        {
            Debug.Log("Walking away");
            obj.GetComponent<NPCMovement>().moveNPC(destination);
        }
        Start();
    }

    public void setupSceneChange(GameObject obj)
    {
        isSceneChanger = true;
        this.sceneName = obj.GetComponent<DialogueTrigger>().sceneName;
        this.battleName = obj.GetComponent<DialogueTrigger>().battleName;
        this.battleSprite = obj.GetComponent<DialogueTrigger>().battleSprite;
        this.battleMaxHealth = obj.GetComponent<DialogueTrigger>().battleMaxHealth;
        this.battleWeaknesses = obj.GetComponent<DialogueTrigger>().battleWeaknesses;
        this.battlePower = obj.GetComponent<DialogueTrigger>().battlePower;
        this.battleAccuracy = obj.GetComponent<DialogueTrigger>().battleAccuracy;
        this.lowHealthBattleDialogue = obj.GetComponent<DialogueTrigger>().lowHealthBattleDialogue;
        this.lowmidHealthBattleDialogue = obj.GetComponent<DialogueTrigger>().lowmidHealthBattleDialogue;
        this.midhighHealthBattleDialogue = obj.GetComponent<DialogueTrigger>().midhighHealthBattleDialogue;
        this.highHealthBattleDialogue = obj.GetComponent<DialogueTrigger>().highHealthBattleDialogue;
        this.battleBackground = obj.GetComponent<DialogueTrigger>().battleBackground;
    }

    public void setupItemGiver(GameObject obj)
    {
        isItemGiver = true;
        this.itemName = obj.GetComponent<DialogueTrigger>().itemName;
        this.itemDescription = obj.GetComponent<DialogueTrigger>().itemDescription;
        this.itemSprite = obj.GetComponent<DialogueTrigger>().itemSprite;
        this.itemCategory = obj.GetComponent<DialogueTrigger>().itemCategory;
        this.attacks = obj.GetComponent<DialogueTrigger>().attacks;
    }
   
    public void setupItemConsumer(GameObject obj)
    {
        isItemConsumer = true;
        if (obj.GetComponent<ItemCheck>() != null)
        {
            this.offset = obj.GetComponent<ItemCheck>().offset;
        }
        this.itemName = obj.GetComponent<DialogueTrigger>().itemName;
        this.itemDescription = obj.GetComponent<DialogueTrigger>().itemDescription;
        this.itemSprite = obj.GetComponent<DialogueTrigger>().itemSprite;
        this.itemCategory = obj.GetComponent<DialogueTrigger>().itemCategory;
    }
    public void setupItemChecker(GameObject obj)
    {
        isItemChecker = true;
        if (obj.GetComponent<ItemCheck>() != null)
        {
            this.offset = obj.GetComponent<ItemCheck>().offset;
        }
        this.itemName = obj.GetComponent<DialogueTrigger>().itemName;
        this.itemDescription = obj.GetComponent<DialogueTrigger>().itemDescription;
        this.itemSprite = obj.GetComponent<DialogueTrigger>().itemSprite;
        this.itemCategory = obj.GetComponent<DialogueTrigger>().itemCategory;
    }
    public void setupWalksAway(GameObject obj)
    {
        this.obj = obj;
        this.walksAway = obj.GetComponent<DialogueTrigger>().walksAway;
        this.destination = obj.GetComponent<DialogueTrigger>().destination;
    }
    public void setupCreateNewObjective(GameObject obj)
    {
        this.obj = obj;
        this.createNewObjective = obj.GetComponent<DialogueTrigger>().createNewObjective;
        this.objectiveName = obj.GetComponent<DialogueTrigger>().objectiveName;
        this.objectiveDescription = obj.GetComponent<DialogueTrigger>().objectiveDescription;
    }

    public void setupProgressBool(string theBool)
    {
        playerProgressBoolToMakeTrue = theBool;
    }
    public void setupDefault(GameObject obj)
    {
        this.obj = obj;
        this.isRemovable = obj.GetComponent<DialogueTrigger>().removableObject;
    }
    public void Reset()
    {
        Start();

    }

}