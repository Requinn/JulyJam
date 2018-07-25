using System.Collections;
using System.Collections.Generic;
using JulyJam.Player;
using MovementEffects;
using UnityEngine;

namespace JulyJam.Interactables {
    /// <summary>
    /// a pair of linked doors for floor movement
    /// </summary>
    public class Door : Interactable{
        public Door linkedDoor;

        /// <summary>
        /// The ladder's interact will start the climb
        /// </summary>
        public override void Interact(PlayerMovement player){
            if (_isInteractable){
                Timing.RunCoroutine(DoClimb(player));
            }
        }

        public override bool Interact(char key){
            //just do NOTHING
            return true;
        }

        /// <summary>
        /// Perform the movement up the stairs or whatever
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> DoClimb(PlayerMovement player){
            _isInteractable = false;
            player.isInteracting = true;
            player.transform.position = transform.position + new Vector3(0, 0, 2f);
            yield return Timing.WaitForSeconds(timeToInteract);
            //LOOK HERE FOR FUCKED UP PLACEMENTS AFTER MOVING
            player.transform.position =
                linkedDoor.transform.position +
                new Vector3(0, -.5f, -2.5f); //+the distance from the middle of the platform to the door
            player.isInteracting = false;
            _isInteractable = true;
            yield return 0f;
        }
    }
}
