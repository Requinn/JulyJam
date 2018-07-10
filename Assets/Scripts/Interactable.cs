﻿using System.Collections;
using System.Collections.Generic;
using JulyJam.Player;
using UnityEngine;

/// <summary>
/// base class for Interactable objects
/// </summary>
public abstract class Interactable : MonoBehaviour{
    [SerializeField] protected float timeToInteract; //how long does it take to interact
    protected bool _isInteractable = true; 
    public abstract void Interact(PlayerMovement player);

}