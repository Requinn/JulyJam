using System.Collections;
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
        [SerializeField] private float lateralSpeed = 1.5f; //how fast we move sideways
        [SerializeField] private float verticalSpeed = 2f; //how fast we climb up and down ladders, Pending Cut
        private CharacterController _Controller;
        private Interactable _currentAccessibleObject; //var to store currently occupied itneractable area
        public bool isInteracting = false;
        private float _storedLateralSpeed; //used to hold the base assigned speed
        private bool isHalted = false; //are we stopped by the ui?

        private float _currentHorizontalInput = 0f;
        private PlayerAnimator _Animator;

        // Use this for initialization

        void Start(){
            _storedLateralSpeed = lateralSpeed;
            _Controller = GetComponent<CharacterController>();
            GameController.Instance.GameOver += HaltInput;
            _Animator = GetComponent<PlayerAnimator>();
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
            bool isSuccessfulHit = false;
            if (_currentAccessibleObject != null && !isHalted){
                //this is for regular interactions
                if (Input.GetKeyDown(KeyCode.Space)){
                    _currentAccessibleObject.Interact(this);
                }
                //send over the char of the key we hit
                if (Input.GetKeyDown(KeyCode.H)){
                    isSuccessfulHit = _currentAccessibleObject.Interact('H');
                }
                else if (Input.GetKeyDown(KeyCode.J)) {
                    isSuccessfulHit = _currentAccessibleObject.Interact('J');
                }
                else if (Input.GetKeyDown(KeyCode.K)) {
                    isSuccessfulHit = _currentAccessibleObject.Interact('K');
                }
                else if (Input.GetKeyDown(KeyCode.L)) {
                    isSuccessfulHit = _currentAccessibleObject.Interact('L');
                }

                //we did a successful repair
                if (isSuccessfulHit){
                    _Animator.PlayHammerStrike();
                }
                else{
                    //do a failure animation
                }
            }
        }

        /// <summary>
        /// prevent the player from moving and doing inputs
        /// </summary>
        public void HaltInput() {
            lateralSpeed = 0;
            isHalted = true;
        }

        /// <summary>
        /// give the controls back
        /// </summary>
        public void ResumeInput(){
            lateralSpeed = _storedLateralSpeed;
            isHalted = false;
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
            _currentHorizontalInput = Input.GetAxis("Horizontal");
            if (!isInteracting){
                _Controller.Move(new Vector3(_currentHorizontalInput * lateralSpeed, 0, 0));
            }
            //Handle the animations for directional movements
            if (_currentHorizontalInput > 0){
                _Animator.UpdateOrientation(true);
                _Animator.UpdateMovement(true);
            }
            if (_currentHorizontalInput < 0){
                _Animator.UpdateOrientation(false);
                _Animator.UpdateMovement(true);
            }
            if (_currentHorizontalInput == 0){
                //_Animator.PlayIdle();
                _Animator.UpdateMovement(false);
            }
        }

    }
}