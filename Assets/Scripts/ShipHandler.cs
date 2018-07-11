using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour{
    public float totalHealth = 1000f;
    private float _absoluteMaxHealth;
    public float currentHealth;
    public RepairableObject[] Rooms;

    public Healthbar UIHealth;
    private float _currentDrain = 0f; //how much hp/s we lose

	// Use this for initialization
	void Start (){
	    currentHealth = _absoluteMaxHealth = totalHealth;
	    UIHealth.UpdateMaxBar(currentHealth, totalHealth, _absoluteMaxHealth);
        foreach (var room in Rooms){
	        //totalHealth += room.parthHealth;
	        room.PartDestroyed += ReduceHealth;
            //maybe we keep drain permanently? or remove when destroyed
	        room.PartBroken += AccumulateDrain;
	        room.PartRepaired += RemoveDrain;
	    }
        InvokeRepeating("HealthDrainTick", 1f, 1f);
	}

    /// <summary>
    /// add onto the drain
    /// </summary>
    /// <param name="drain"></param>
    private void AccumulateDrain(float drain){
        _currentDrain += drain;
    }

    /// <summary>
    /// remove from the drain
    /// </summary>
    /// <param name="drain"></param>
    private void RemoveDrain(float drain){
        _currentDrain -= drain;
    }

    /// <summary>
    /// Take damage from a part being destroyed
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="rObject"></param>
    private void ReduceHealth(float damage, RepairableObject rObject){
        rObject.PartBroken -= AccumulateDrain;
        rObject.PartRepaired -= RemoveDrain;
        totalHealth -= damage;
        currentHealth -= damage;
        UIHealth.UpdateMaxBar(currentHealth, totalHealth, _absoluteMaxHealth);
    }

    /// <summary>
    /// repeatedly invoked function to tick health down
    /// </summary>
    private void HealthDrainTick(){
        currentHealth -= _currentDrain;
        UIHealth.UpdateMaxBar(currentHealth, totalHealth, _absoluteMaxHealth);
        //UI update to show health falling
    }

    // Update is called once per frame
	void Update () {
		
	}
}
