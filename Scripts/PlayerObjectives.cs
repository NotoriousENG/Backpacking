using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectives : MonoBehaviour
{

    static PlayerObjectives instance;

    public Phone phone;
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
        phone = new Phone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
