
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    public Dialogue lowHealthBattleDialogue;
    public Dialogue lowmidHealthBattleDialogue;
    public Dialogue midhighHealthBattleDialogue;
    public Dialogue highHealthBattleDialogue;

    public bool isSceneChanger;
    public string sceneName;
    public string battleName;
    public Sprite battleSprite;
    public int battleMaxHealth;
    public string[] battleWeaknesses;
    public int battlePower;
    public float battleAccuracy;

    public bool isItemGiver;
    public bool isItemConsumer;
    public bool isItemChecker;
    public bool isAutoTalker;
    public string playerProgressBoolToMakeTrue;
    public bool createNewObjective;
    public string objectiveName;
    public string objectiveDescription;

    public bool removableObject;
    public bool walksAway;
    public Vector3 destination;

    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public string itemCategory;
    public Attack[] attacks;

    public GameObject interactable;

    public Sprite battleBackground;

    public void TriggerDialogue()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.Reset();
            if (isSceneChanger)
            {
                dialogueManager.setupSceneChange(interactable);
            }
            if (isItemGiver)
            {
                dialogueManager.setupItemGiver(interactable);
            }
            if (isItemConsumer)
            {
                dialogueManager.setupItemConsumer(interactable);
            }
            if (isItemChecker)
            {
                dialogueManager.setupItemChecker(interactable);
            }
            if (walksAway)
            {
                dialogueManager.setupWalksAway(interactable);
            }
            if (createNewObjective)
            {
                dialogueManager.setupCreateNewObjective(interactable);
            }
            if (playerProgressBoolToMakeTrue != "")
            {
                dialogueManager.setupProgressBool(playerProgressBoolToMakeTrue);
            }
            dialogueManager.setupDefault(interactable);
            dialogueManager.StartDialogue(dialogue);
        }
    }

    public void ClearFields()
    {
        dialogue = new Dialogue();
        isSceneChanger = false;
        sceneName = "";
        battleName = "";
        battleSprite = null;
        battleMaxHealth = 0;
        battleWeaknesses = null;
        battlePower = 0;
        battleAccuracy = 0.0f;

        isItemGiver = false;
        isItemConsumer = false;
        isItemChecker = false;
        isAutoTalker = false;
        playerProgressBoolToMakeTrue = "";

        removableObject = false;

        itemName = "";
        itemDescription = "";
        itemSprite = null;
        itemCategory = "";
        attacks = null;
}

}