using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle the score for the game
/// </summary>
public class ScoreUpdater : MonoBehaviour{
    public Text ScoreText;
    public readonly Transform[] ScoreIncrementPositions;
    private int _currentScore = 0;
	// Use this for initialization
	void Start (){
	    UpdateText();
	}

    public void AddToScore(int Amount){
        _currentScore += Amount;
        UpdateText();
    }

    private void UpdateText(){
        ScoreText.text = _currentScore.ToString();
    }
}
