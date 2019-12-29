using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachine : MonoBehaviour
{
    private GameObject washerBattle;
    private GameObject bat;
    PlayerProgress playerProgress;
    // Start is called before the first frame update
    void Start()
    {
        washerBattle = GameObject.Find("Bat + Washer Battle");
        bat = GameObject.Find("bat");
        playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();

        if(playerProgress.washerDefeated)
        {
            Destroy(bat);
            washerBattle.GetComponent<DialogueTrigger>().enabled=false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
