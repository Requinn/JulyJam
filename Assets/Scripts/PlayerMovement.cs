﻿using System.Collections;
using System.Collections.Generic;
using JulyJam.Core;
using JulyJam.Interactables;
using UnityEngine;
using UnityScript.Steps;

namespace JulyJam.Player{
    /// <summary>
    /// Class for controlling the player movements
    /// </summary>
    public class PlayerMovement : MonoBehaviour{
        [SerializeField] private float lateralSpeed = 7f; //how fast we move sideways
        [SerializeField] private float verticalSpeed = 2f; //how fast we climb up and down ladders, Pending Cut
        private CharacterController _Controller;
        private Interactable _currentAccessibleObject;
        public bool isInteracting = false;

        //var to store currently occupied itneractable area
        // Use this for initialization

        void Start(){
            _Controller = GetComponent<CharacterController>();
            GameController.Instance.GameOver += HaltInput;
        }

        // Update is called once per frame
        void Update(){
            GetMovementInput();
            GetInteractInput();
        }

        /// <summary>
        /// get input for interaction
        /// </summary>
        private void GetInteractInput(){
            //only bother checking if we have an object to interact with
            if (_currentAccessibleObject != null){
                //this is for regular interactions
                if (Input.GetKeyDown(KeyCode.Space)){
                    _currentAccessibleObject.Interact(this);
                }
                //send over the char of the key we hit
                if (Input.GetKeyDown(KeyCode.H)){
                    _currentAccessibleObject.Interact('H');
                }
                if (Input.GetKeyDown(KeyCode.J)) {
                    _currentAccessibleObject.Interact('J');
                }
                if (Input.GetKeyDown(KeyCode.K)) {
                    _currentAccessibleObject.Interact('K');
                }
                if (Input.GetKeyDown(KeyCode.L)) {
                    _currentAccessibleObject.Interact('L');
                }

            }
        }

        /// <summary>
        /// Game is over, just set our speed to 0;
        /// </summary>
        private void HaltInput() {
            lateralSpeed = 0;
        }

        void OnTriggerEnter(Collider c){
            _currentAccessibleObject = c.GetComponent<Interactable>();
        }

        void OnTriggerExit(Collider c){
            _currentAccessibleObject = null;
        }

        /// <summary>
        /// Get input for movement
        /// </summary>
        void GetMovementInput(){
            //Check if we're interacting
            if (!isInteracting) {
                _Controller.Move(new Vector3(Input.GetAxis("Horizontal") * lateralSpeed, 0, 0));
            }
        }
    }
}