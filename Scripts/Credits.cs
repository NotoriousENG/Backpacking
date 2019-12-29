using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    private float timeInCredits = 0f;

    // Update is called once per frame
    void Update()
    {
        timeInCredits += Time.deltaTime;

        if(timeInCredits > 30)
        {
            SceneManager.LoadScene("Start Menu");
        }
    }
}
