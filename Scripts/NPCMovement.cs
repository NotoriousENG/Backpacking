using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    //private Animator moveM;
    [SerializeField] private float distanceToMove = 1;
    [SerializeField] private float moveSpeed = 5;
    private bool moveToPoint;
    public bool doneMoving;
    private string facing;
    private Vector3 endPosition;
    private Vector3 startPosition;
    private GameObject obj;
    bool shouldMoveLeft;
    bool shouldMoveRight;
    bool shouldMoveUp;
    bool shouldMoveDown;
    public bool randomMovement = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //moveM = GetComponent<Animator>();
        endPosition = transform.localPosition;
        moveToPoint = false;
        doneMoving = true;
        shouldMoveLeft = false;
        shouldMoveRight = false;
        shouldMoveUp = false;
        shouldMoveDown = false;
        if (randomMovement)
        {
            StartCoroutine(movementGenerator());
        }
    }
    void FixedUpdate()
    {
        if (moveToPoint && obj==null)
        {
            //Debug.Log("Moving");
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPosition, moveSpeed * Time.deltaTime);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition == endPosition)
        {
            doneMoving = true;
            moveToPoint = false;
            //moveM.SetFloat("MoveX", 0);
            //moveM.SetFloat("MoveY", 0);
        }
        if (doneMoving == true)
        {
            if (shouldMoveLeft == true) //Left
            {
                //moveM.SetFloat("MoveX", -1);
                //moveM.SetFloat("MoveY", 0);
                startPosition = endPosition;
                endPosition = new Vector3(endPosition.x - distanceToMove, endPosition.y, endPosition.z);
                moveToPoint = true;
                doneMoving = false;
                facing = "left";
                shouldMoveLeft = false;

            }
            if (shouldMoveRight == true) //Right
            {
                //moveM.SetFloat("MoveX", 1);
                //moveM.SetFloat("MoveY", 0);
                startPosition = endPosition;
                endPosition = new Vector3(endPosition.x + distanceToMove, endPosition.y, endPosition.z);
                moveToPoint = true;
                doneMoving = false;
                facing = "right";
                shouldMoveRight = false;

            }
            if (shouldMoveUp == true) //Up
            {
                //moveM.SetFloat("MoveX", 0);
                //moveM.SetFloat("MoveY", 1);
                startPosition = endPosition;
                endPosition = new Vector3(endPosition.x, endPosition.y + distanceToMove, endPosition.z);
                moveToPoint = true;
                doneMoving = false;
                facing = "up";
                shouldMoveUp = false;

            }
            if (shouldMoveDown == true) //Down
            {
                //moveM.SetFloat("MoveX", 0);
                //moveM.SetFloat("MoveY", -1);
                startPosition = endPosition;
                endPosition = new Vector3(endPosition.x, endPosition.y - distanceToMove, endPosition.z);
                moveToPoint = true;
                doneMoving = false;
                facing = "down";
                shouldMoveDown = false;

            }
        }
    }
    //Callback when enter the collision
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        endPosition = startPosition;
        moveToPoint = true;
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            obj = collision.gameObject;
            obj.GetComponent<PlayerMovement>().cancelPlayerMove();
            endPosition = startPosition;
            moveToPoint = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            obj = null;
        }
    }

    public void moveNPC(Vector3 newPosition)
    {
        endPosition = newPosition;
        moveToPoint = true;
        doneMoving = false;
    }

    IEnumerator movementGenerator()
    {
        while (true)
        {
            Debug.Log("movegen");
            if (doneMoving == true)
            {
                //shouldMoveLeft = false;
                //shouldMoveRight = false;
                //shouldMoveUp = false;
                //shouldMoveDown = false;
                float direction = Random.Range(0, 5);
                Debug.Log(direction);
                switch (direction)
                {
                    case 0:
                        shouldMoveLeft = true;
                        break;
                    case 1:
                        shouldMoveRight = true;
                        break;
                    case 2:
                        shouldMoveUp = true;
                        break;
                    case 3:
                        shouldMoveDown = true;
                        break;
                    default:
                        break;
                }
            }

            yield return new WaitForSeconds(2.0f);
        }
    }
}
