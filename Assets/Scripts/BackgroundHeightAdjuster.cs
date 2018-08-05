using System.Collections;
using System.Collections.Generic;
using JulyJam.Core;
using UnityEngine;

/// <summary>
/// move the background sky based on the current health of the ship 
/// </summary>
public class BackgroundHeightAdjuster : MonoBehaviour{
    public ShipHandler ship;
    public GameObject backgroundObject;
    [SerializeField] private Vector3 _minimumPosition; //the lowest we can go
    private Vector3 _homePosition;

    void Start(){
        _homePosition = backgroundObject.transform.localPosition; //get the original position
        _minimumPosition = new Vector3(0,88,0); //a test value
    }

    void Update(){
        AdjustBackground();
    }

    /// <summary>
    /// Based on ship HP, move the "altitude" up and down smoothly
    /// </summary>
    private void AdjustBackground(){
        float healthPercent = ship.GetShipHealthPercent();
        float goalHeight = Mathf.Lerp(_minimumPosition.y, _homePosition.y, healthPercent); //get where we should be moving to
        float lerpedHeight = Mathf.Lerp(backgroundObject.transform.position.y, goalHeight, Time.deltaTime); //lerp our current position to that position
        backgroundObject.transform.position = new Vector3(_homePosition.x, lerpedHeight, _homePosition.z); //apply the lerp
    }
}
