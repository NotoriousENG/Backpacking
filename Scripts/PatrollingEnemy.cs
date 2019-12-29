using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PatrollingEnemy : MonoBehaviour
{
    public float speed;
    private CircleCollider2D patrolRadius;
    private bool isFollowing;
    private GameObject playerGO;
    private float timeClose = 0f;


    // Start is called before the first frame update
    void Start()
    {
        patrolRadius = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            Target(playerGO);
        }
    }

    void Target(GameObject player)
    {
        Vector3 newScale = transform.localScale; // newScale.x is positive, facing left
        if (player.transform.position.x < transform.position.x) // if player is to the left
        {
            if (newScale.x < 0) // if facing the wrong way (right)
            {
                newScale.x *= -1;
                transform.localScale = newScale; // face the right way (left)
            }
        }
        if (player.transform.position.x > transform.position.x) // if player is to the right
        {
            if (newScale.x > 0) // if facing the wrong way (left)
            {
                newScale.x *= -1;
                transform.localScale = newScale; // face the right way (right)
            }
        }

        if (Math.Abs(player.transform.position.x - transform.position.x) <= 1.5 && Math.Abs(player.transform.position.y - transform.position.y) <=1.5)
        { // if the doggo is close
            timeClose += Time.deltaTime;
            if(timeClose > .25)
            {
                gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            }
            
        }
        else
        {
            timeClose = 0f;
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
           
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerGO = other.gameObject;
            isFollowing = true;
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isFollowing = false;
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        }
    }
}
