using System.Collections;
using System.Collections.Generic;
using JulyJam.Player;
using MovementEffects;
using UnityEngine;

public class RepairableObject : Interactable{
    [SerializeField] private float _malfunctionInterval; //how often we roll a check
    [SerializeField] private float _malfunctionWeight; //whats the chance we break on a check out of 100
    [SerializeField] private bool _isBroken;
    public GameObject needsRepairMarker;
    private float _timeSinceLastCheck = 0f;

    void Start(){
        needsRepairMarker.SetActive(false);
    }

	// Update is called once per frame
	void Update (){
	    if (!_isBroken){ //if we aren't broken
	        _timeSinceLastCheck += Time.deltaTime;
	        if (_timeSinceLastCheck >= Mathf.FloorToInt(_malfunctionInterval)){ //check if its time to roll
	            int rng = Random.Range(0, 101); 
	            if (rng < _malfunctionWeight){ //do the roll and check
	                BreakObject(); //we broke
	            }
	            _timeSinceLastCheck = 0;
            }
	    }
	}

    /// <summary>
    /// called to break the object
    /// </summary>
    public void BreakObject(){
        _isBroken = true;
        needsRepairMarker.SetActive(true);
        //do some fx and sprite swaps in here
    }

    /// <summary>
    /// caled to fix the object
    /// </summary>
    public void RepairObject(){
        needsRepairMarker.SetActive(false);
        _isBroken = false;
    }

    /// <summary>
    /// initiate repairs
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(PlayerMovement player){
        if (_isInteractable && _isBroken){
            _isInteractable = false;
            player.isInteracting = true;
            Timing.RunCoroutine(DoRepair(player));
        }
    }

    /// <summary>
    /// the act of repairing
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private IEnumerator<float> DoRepair(PlayerMovement player){
        yield return Timing.WaitForSeconds(timeToInteract);
        _isInteractable = true;
        player.isInteracting = false;
        RepairObject();
        yield return 0f;
    }
}
