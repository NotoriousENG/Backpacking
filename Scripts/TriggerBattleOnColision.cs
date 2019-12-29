using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerBattleOnColision : MonoBehaviour
{
    private DialogueTrigger DialogueTrigger;
    private CapsuleCollider2D capsule;
    private bool theBattleTrigger = false;
    private bool triggerUsed = false;
    private bool isDone = false;
    private GameObject enemy;

    private void Start()
    {
        DialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        capsule = gameObject.GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if(theBattleTrigger == true)
        {
            DialogueTrigger.TriggerDialogue();
            //SceneManager.LoadScene("Battle");
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !triggerUsed)
        {
            theBattleTrigger = true;
            enemy = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player" && triggerUsed)
        {
            //theBattleTrigger = false;
        }
    }
}
