using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamera : MonoBehaviour
{
    GameObject player;
    CinemachineVirtualCamera followCam;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject player = FindObjectOfType<GameObject>(); // might return null!
        player = GameObject.FindGameObjectWithTag("Player");
        followCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        followCam.Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        followCam.Follow = player.transform;
    }
}
