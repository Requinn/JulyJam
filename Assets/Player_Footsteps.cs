using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Footsteps : MonoBehaviour {

    [FMODUnity.EventRef]
    public string PlayEvent;
    bool playerismoving;
    public float walkingspeed;
    
    void Update ()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            //Debug.Log("player is moving");
            playerismoving = true;
        }
        else if (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
        {
            //Debug.Log("Player is not moving");
            playerismoving = false;
        }
        }

        void CallFootsteps ()
    {
        if (playerismoving == true)
        {
            //Debug.Log ("player is moving");
            FMODUnity.RuntimeManager.PlayOneShot(PlayEvent);

        }
    }

    void Start ()
    {
        InvokeRepeating("CallFootsteps", 0, walkingspeed);
    }
    
    void OnDisable ()
    {
        playerismoving = false;
    }
    }
    

