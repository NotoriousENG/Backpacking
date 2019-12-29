//Free movement
/*using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField]
    private float speedMultiplier;
    private Vector2 vectorHorizontal;
    private Vector2 vectorVertical;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        vectorHorizontal = new Vector2(1.0f, 0.0f);
        vectorVertical = new Vector2(0.0f, 1.0f);
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            rb2d.MovePosition(rb2d.position + vectorHorizontal);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rb2d.MovePosition(rb2d.position - vectorHorizontal);
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            rb2d.MovePosition(rb2d.position + vectorVertical);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rb2d.MovePosition(rb2d.position - vectorVertical);
        }
    }
}*/

//Grid-based Movement
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayerMovement : MonoBehaviour
{
    public Animator moveM;
    [SerializeField] private float distanceToMove;
    [SerializeField] private float moveSpeed;
    private bool moveToPoint;
    public bool doneMoving;
    public string facing;
    private Vector3 endPosition;
    private Vector3 startPosition;
    private AudioSource audioSrc;
    private float originalMoveSpeed;
    public GameObject door;

    private float keyDelay = 0.075f;
    private float timePassed = 0f;

    public bool justStartedGame;

    public GameObject dialogueCollider;

    private bool teleporting;

    private float timeStill = 0f;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    void Start()
    {
        moveM = GetComponent<Animator>();
        Debug.Log("Text: " + "start");
        endPosition = transform.position;
        moveToPoint = false;
        doneMoving = true;
        justStartedGame = true;
        originalMoveSpeed = moveSpeed;
        teleporting = false;
    }

    void FixedUpdate()
    {
        if (moveToPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);
        }
    }
    void Update()
    {
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            moveM.SetBool("isWalking", false);
        }

        timePassed += Time.deltaTime;
        //Debug.Log("Time passed: " + timePassed);

        if (gameObject.GetComponent<PlayerInventory>().backpack.hasItem("Giant Donut"))
        {
            GameObject giantDonut = GameObject.Find("Giant Donut");
            giantDonut.GetComponent<BoxCollider2D>().enabled = false;
            giantDonut.transform.localScale = new Vector3(2f, 2f, 2f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, 12f);
            giantDonut.transform.localRotation = rotation;
            Vector3 offset = new Vector3();
            if (facing == "right")
            {
                offset = new Vector3(-0.6f, 0.5f, 0f);
                giantDonut.GetComponent<SpriteRenderer>().sortingLayerName = "BehindPlayer";
            }
            if (facing == "left")
            {
                offset = new Vector3(0.6f, 0.5f, 0f);
                giantDonut.GetComponent<SpriteRenderer>().sortingLayerName = "BehindPlayer";
                rotation = Quaternion.Euler(0f, 0f, -12f);
                giantDonut.transform.localRotation = rotation;
            }
            if (facing == "up")
            {
                offset = new Vector3(0f, 0.5f, 0f);
                giantDonut.GetComponent<SpriteRenderer>().sortingLayerName = "AbovePlayer";
            }
            if (facing == "down")
            {
                offset = new Vector3(0f, 0.5f, 0f);
                giantDonut.GetComponent<SpriteRenderer>().sortingLayerName = "BehindPlayer";
            }
            giantDonut.transform.position = transform.position + offset;
        }

        if (transform.position == endPosition && teleporting == false)
        { 
            doneMoving = true;
            moveToPoint = false;
            moveSpeed = originalMoveSpeed;
            moveM.SetFloat("MoveX", 0);
            moveM.SetFloat("MoveY", 0);
        }
        if (doneMoving == true)
        {
            //Change player's facing direction (without moving)
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                moveM.Play("Player_Idle_Left");
                moveM.SetBool("isWalking", false);
                moveM.SetBool("isUp", false);
                moveM.SetBool("isRight", false);
                moveM.SetBool("isDown", false);

                moveM.SetBool("isLeft" , true);
                dialogueCollider.transform.localPosition = new Vector3(-1.25f, 0.25f, 0);
                facing = "left";
                timePassed = 0f;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                moveM.Play("Player_Idle_Right");
                moveM.SetBool("isWalking", false);
                moveM.SetBool("isUp", false);
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isDown", false);

                moveM.SetBool("isRight", true);
                dialogueCollider.transform.localPosition = new Vector3(-0.75f, 0.25f, 0);
                facing = "right";
                timePassed = 0f;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                moveM.Play("Player_Idle_Up");
                moveM.SetBool("isWalking", false);
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isRight", false);
                moveM.SetBool("isDown", false);

                moveM.SetBool("isUp", true);
                dialogueCollider.transform.localPosition = new Vector3(-1, 0.5f, 0);
                facing = "up";
                timePassed = 0f;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow))
            {
                //Debug.Log("MADE IT!!!!!!!!!!!!!!");
                moveM.Play("Player_Idle_Down");
                moveM.SetBool("isWalking", false);
                moveM.SetBool("isUp", false);
                moveM.SetBool("isRight", false);
                moveM.SetBool("isLeft", false);

                moveM.SetBool("isDown", true);
                dialogueCollider.transform.localPosition = new Vector3(-1, 0, 0);
                facing = "down";
                timePassed = 0f;
            }
            //Change player's facing direction (and move)
            if (Input.GetKey(KeyCode.LeftArrow) && timePassed >= keyDelay && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) //Left
            {
                moveM.SetFloat("MoveX", -1);
                moveM.SetFloat("MoveY", 0);

                moveM.SetBool("isUp", false);
                moveM.SetBool("isRight", false);
                moveM.SetBool("isDown", false);

                moveM.SetBool("isLeft", true);
                moveM.SetBool("isWalking", true);
                startPosition = endPosition;
                endPosition = new Vector3(endPosition.x - distanceToMove, endPosition.y, endPosition.z);
                moveToPoint = true;
                doneMoving = false;
            }
            if (Input.GetKey(KeyCode.RightArrow) && timePassed >= keyDelay && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) //Right
            {
                moveM.SetFloat("MoveX", 1);
                moveM.SetFloat("MoveY", 0);

                moveM.SetBool("isUp", false);
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isDown", false);

                moveM.SetBool("isRight", true);
                moveM.SetBool("isWalking", true);
                startPosition = endPosition;
                endPosition = new Vector3(endPosition.x + distanceToMove, endPosition.y, endPosition.z);
                moveToPoint = true;
                doneMoving = false;
            }
            if (Input.GetKey(KeyCode.UpArrow) && timePassed >= keyDelay && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow)) //Up
            {
                moveM.SetFloat("MoveX", 0);
                moveM.SetFloat("MoveY", 1);

                moveM.SetBool("isRight", false);
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isDown", false);

                moveM.SetBool("isUp", true);
                moveM.SetBool("isWalking", true);

                startPosition = endPosition;
                endPosition = new Vector3(endPosition.x, endPosition.y + distanceToMove, endPosition.z);
                moveToPoint = true;
                doneMoving = false;
            }
            if (Input.GetKey(KeyCode.DownArrow) && timePassed >= keyDelay && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow)) //Down
            {
                moveM.SetFloat("MoveX", 0);
                moveM.SetFloat("MoveY", -1);

                moveM.SetBool("isRight", false);
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isUp", false);

                moveM.SetBool("isDown", true);
                moveM.SetBool("isWalking", true);

                startPosition = endPosition;
                endPosition = new Vector3(endPosition.x, endPosition.y - distanceToMove, endPosition.z);
                moveToPoint = true;
                doneMoving = false;
            }
        }
        else
        {
            // moveM.SetBool("isWalking", false);

            if (Input.GetKeyDown(KeyCode.LeftArrow) && timePassed >= keyDelay && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) //Left
            {
                moveM.SetBool("isRight", false);
                moveM.SetBool("isDown", false);
                moveM.SetBool("isUp", false);

                moveM.SetBool("isLeft", true);

                moveM.Play("Player_Idle_Left");
                moveM.SetFloat("MoveX", -1);
                moveM.SetFloat("MoveY", 0);
                moveM.SetBool("isWalking", true);
                facing = "left";
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && timePassed >= keyDelay && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) //Right
            {
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isDown", false);
                moveM.SetBool("isUp", false);

                moveM.SetBool("isRight", true);

                moveM.Play("Player_Idle_Right");
                moveM.SetFloat("MoveX", 1);
                moveM.SetFloat("MoveY", 0);
                moveM.SetBool("isWalking", true);
                facing = "right";
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && timePassed >= keyDelay && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow)) //Up
            {
                moveM.SetBool("isRight", false);
                moveM.SetBool("isDown", false);
                moveM.SetBool("isLeft", false);

                moveM.SetBool("isUp", true);

                moveM.Play("Player_Idle_Up");
                moveM.SetFloat("MoveX", 0);
                moveM.SetFloat("MoveY", 1);
                moveM.SetBool("isWalking", true);
                facing = "up";
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && timePassed >= keyDelay && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow)) //Down
            {
                moveM.SetBool("isRight", false);
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isUp", false);

                moveM.SetBool("isDown", true);

                moveM.Play("Player_Idle_Down");
                moveM.SetFloat("MoveX", 0);
                moveM.SetFloat("MoveY", -1);
                moveM.SetBool("isWalking", true);
                facing = "down";
            }
            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) //Left
            {
                facing = "left";
            }
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) //Right
            {
                facing = "right";
            }
            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow)) //Up
            {
                facing = "up";
            }
            if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow)) //Down
            {
                facing = "down";
            }
        }
    }
    //Callback when enter the collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Text: " + "collision");
        endPosition = startPosition;
        moveToPoint = true;
        if (!audioSrc.isPlaying)
        {
            audioSrc.Play();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            door = collision.gameObject;
            Vector3 telePos = door.GetComponent<Door>().telePos;
            facing = door.GetComponent<Door>().facingDirection;
            //PlayerMovement player = transform.parent.gameObject.GetComponent<PlayerMovement>();
           Teleport(telePos, facing);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            door = null;
            //Vector3 x = door.GetComponent<Door>().telePos;
            //PlayerMovement player = transform.parent.gameObject.GetComponent<PlayerMovement>();
            //teleportDownstairs(x);
        }
    }

    public void Teleport(Vector3 telePos, string facingDirection)
    {
        StartCoroutine(fadeToBlackAndBack(facingDirection, 2));
        Debug.Log("Teleporting");
        endPosition = telePos;
        moveSpeed = 10000;
        moveToPoint = true;
        doneMoving = false;
        teleporting = true;
    }

        

    int currCountdownValue;
    public IEnumerator fadeToBlackAndBack(string facingDirection, int countdownValue = 2)
    {
        SpriteRenderer black = GameObject.Find("Black").GetComponent<SpriteRenderer>();
        black.color = new Color(0, 0, 0, 1);
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            //Makes sure player is not animated as if moving
            moveM.SetBool("isWalking", false);
            facing = facingDirection;

            moveM.SetFloat("MoveX", 0);
            moveM.SetFloat("MoveY", 0);
            if (facing == "up")
            {
                moveM.Play("Player_Idle_Up");
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isRight", false);
                moveM.SetBool("isUp", true);
                moveM.SetBool("isDown", false);
                dialogueCollider.transform.localPosition = new Vector3(-1, 0.5f, 0);
            }
            if (facing == "down")
            {
                moveM.Play("Player_Idle_Down");
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isRight", false);
                moveM.SetBool("isUp", false);
                moveM.SetBool("isDown", true);
                dialogueCollider.transform.localPosition = new Vector3(-1, 0, 0);
            }
            if (facing == "left")
            {
                moveM.Play("Player_Idle_Left");
                moveM.SetBool("isLeft", true);
                moveM.SetBool("isRight", false);
                moveM.SetBool("isUp", false);
                moveM.SetBool("isDown", false);
                dialogueCollider.transform.localPosition = new Vector3(-1.25f, 0.25f, 0);
            }
            if (facing == "right")
            {
                moveM.Play("Player_Idle_Right");
                moveM.SetBool("isLeft", false);
                moveM.SetBool("isRight", true);
                moveM.SetBool("isUp", false);
                moveM.SetBool("isDown", false);
                dialogueCollider.transform.localPosition = new Vector3(-0.75f, 0.25f, 0);
            }

            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
            if (currCountdownValue == 0)
            {
                black.color = new Color(0, 0, 0, 0);
                teleporting = false;
            }
        }
    }

    public void movePlayer(Vector3 newPosition)
    {
        endPosition = newPosition;
        moveToPoint = true;
        doneMoving = false;
    }

    public void cancelPlayerMove()
    {
        endPosition = startPosition;
        transform.position = endPosition;
        moveToPoint = false;
        doneMoving = true;
        moveM.SetBool("isWalking", false);
    }

}