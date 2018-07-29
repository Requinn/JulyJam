using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// convert a string into a GO display representation
/// </summary>
public class RepairableInputUI : MonoBehaviour{
    //an array of an object containing all 4 letters
    public RepairableInputLetter[] inputLetters;
    //only 3 irectional arrows
    public GameObject[] directionalArrows;

    /// <summary>
    /// Taking a string arg, use it to show the input through a gameobject
    /// </summary>
    /// <param name="Input"></param>
    public void DisplayInputString(string Input){
        int _letterIndex = 0; //which letter are we on
        int _directionalIndex = 0; //which arrow are we on

        for (int i = 0; i < Input.Length; i++){
            if (Input[i] != '>'){
                inputLetters[_letterIndex].DisplayLetter(Input[i]);
                _letterIndex++;
            }
            else { 
                directionalArrows[_directionalIndex].SetActive(true);
                _directionalIndex++;
            }
        }
    }

    /// <summary>
    /// Disable all active strings
    /// </summary>
    public void DisableAll(){
        foreach (var v in inputLetters){
            v.DisableChar();
        }
        directionalArrows[0].SetActive(false);
        directionalArrows[1].SetActive(false);
        directionalArrows[2].SetActive(false);
    }
}
