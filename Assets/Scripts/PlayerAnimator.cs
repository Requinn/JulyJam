using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour{
    public Animator AnimationController;
    private bool _localIsRightFacing;

	// Use this for initialization
	void Start () {
	    if (AnimationController == null){
	        AnimationController = GetComponent<Animator>();
	    }	
	}

    /// <summary>
    /// tell the animator if we are facing right or not
    /// </summary>
    /// <param name="isFacingRight"></param>
    public void UpdateOrientation(bool isFacingRight){
        AnimationController.SetBool("isFacingRight", isFacingRight);
        _localIsRightFacing = isFacingRight;
    }

    /// <summary>
    /// tell the animator if we are moving
    /// </summary>
    /// <param name="isMoving"></param>
    public void UpdateMovement(bool isMoving){
        AnimationController.SetBool("isMoving", isMoving);
    }

    /// <summary>
    /// play the hammer animation
    /// </summary>
    /// <param name="animationName"></param>
    public void PlayHammerStrike(){
        if (_localIsRightFacing){
            AnimationController.Play("PlayerHammerRight");
        }
        else{
            AnimationController.Play("PlayerHammerLeft");
        }
    }

}
