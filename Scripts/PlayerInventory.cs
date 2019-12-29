using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    static PlayerInventory instance;

    public Backpack backpack;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else if (instance != this)
        {
            Destroy(transform.gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        //Maybe need to comment out
        backpack = new Backpack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
