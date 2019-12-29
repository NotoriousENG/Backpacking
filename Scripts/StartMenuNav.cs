using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuNav : MonoBehaviour
{
    public GameObject leftCursor;
    public GameObject rightCursor;
    private bool isRight = false;
    public Sprite Sprite;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) // if we move the cursor
        {
            isRight = !isRight; // cursor needs to go to the other position
        }

        if (isRight) // cursor is on the right
        {
            leftCursor.SetActive(false);
            rightCursor.SetActive(true);
        }

        if (!isRight) // cursor is on the left
        {
            rightCursor.SetActive(false);
            leftCursor.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isRight) // if credits is selected
            {
                rightCursor.GetComponent<Image>().sprite = Sprite; // change the sprite to open
                SceneManager.LoadScene("Credits");
            }
            if (!isRight) // if start is selected
            {
                leftCursor.GetComponent<Image>().sprite = Sprite; // change the sprite to open
                SceneManager.LoadScene("Game World");
            }
        }
    }
}
