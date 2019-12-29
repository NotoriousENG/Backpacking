using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAllObjectsBetweenScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
