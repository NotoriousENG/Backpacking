using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickTree : MonoBehaviour
{
    private GameObject brownStick;

    // Start is called before the first frame update
    void Start()
    {
        brownStick = GameObject.Find("Sticky");
        brownStick.GetComponent<DialogueTrigger>().enabled = false;
       
    }

    // Update is called once per frame
    void Update()
    {

    }
}