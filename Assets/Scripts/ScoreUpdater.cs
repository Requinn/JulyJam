using System.Collections;
using System.Collections.Generic;
using JulyJam.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handle the score for the game
/// </summary>
public class ScoreUpdater : MonoBehaviour{
    public Text ScoreText;
    public Text FinalScoreText;
    public readonly Transform[] ScoreIncrementPositions;
    private int _currentScore = 0;

	// Use this for initialization
	void Start (){
	    GetComponent<GameController>().GameOver += FinalizeScoreToScreen;
	    UpdateText();
	}

    /// <summary>
    /// Finalize our score in the last screen we see
    /// </summary>
    private void FinalizeScoreToScreen(){
        FinalScoreText.text = _currentScore.ToString();
    }

    /// <summary>
    /// Increment our score
    /// </summary>
    /// <param name="Amount"></param>
    public void AddToScore(int Amount){
        _currentScore += Amount;
        UpdateText();
    }

    /// <summary>
    /// update the score ui
    /// </summary>
    private void UpdateText(){
        if (ScoreText != null){
            ScoreText.text = _currentScore.ToString();
        }
    }

    /// <summary>
    /// Save the score to a local leaderboard, passing in the time survived on the level as a part of the score
    /// </summary>
    public void FinalizeToLeaderBoard(int totalTime){
        LocalLeaderboardsHandler.Instance.CheckAgainstLeaderBoard(new LeaderBoardEntry(SceneManager.GetActiveScene().name, totalTime, _currentScore));
    }
}
