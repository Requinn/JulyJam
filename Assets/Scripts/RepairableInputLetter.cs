using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// given a char input, show a particular GO with the letter on it
/// </summary>
public class RepairableInputLetter : MonoBehaviour{
    //H J K L
    public GameObject[] validInputs;

    /// <summary>
    /// display a gameobject of the letter
    /// </summary>
    /// <param name="letter"></param>
    public void DisplayLetter(char letter){
        if (letter == 'H'){
            validInputs[0].SetActive(true);
        }
        if (letter == 'J'){
            validInputs[1].SetActive(true);
        }
        if (letter == 'K'){
            validInputs[2].SetActive(true);
        }
        if (letter == 'L'){
            validInputs[3].SetActive(true);
        }
    }

    /// <summary>
    /// disable all characters
    /// </summary>
    public void DisableChar(){
        validInputs[0].SetActive(false);
        validInputs[1].SetActive(false);
        validInputs[2].SetActive(false);
        validInputs[3].SetActive(false);
    }
}
