using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 telePos;
    public string facingDirection;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            player = collision.gameObject;
            player = collision.transform.parent.gameObject.GetComponent<PlayerMovement>;

            //PlayerMovement player = transform.parent.gameObject.GetComponent<PlayerMovement>();
            player.teleportDownstairs(telePos);
        }
    }*/
}
