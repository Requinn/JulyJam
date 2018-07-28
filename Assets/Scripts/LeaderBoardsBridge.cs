using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bridges the leaderboards class to the UI of the main menu
/// </summary>
public class LeaderBoardsBridge : MonoBehaviour {
    public PlacingsGroup[] ScoreUIElements;

    public void LoadScores(){
        DisplayScores(LocalLeaderboardsHandler.Instance.leaderBoards);
    }

    /// <summary>
    /// Show all the scores
    /// </summary>
    public void DisplayScores(List<LeaderBoardEntry> data) {
        if (data.Count > 0) {
            for (int i = 0; i < data.Count; i++) {
                ScoreUIElements[i].levelName.text = data[i].levelName;
                ScoreUIElements[i].levelTime.text = ConvertIntToTime(data[i].timeSpent);
                ScoreUIElements[i].finalScore.text = data[i].finalScore.ToString();
            }
        }
    }

    /// <summary>
    /// given an integer value of seconds passed, convert it into a string format for display
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private string ConvertIntToTime(int time) {
        int minutes = time / 60;
        int seconds = time % 60;
        string fixedSeconds = seconds.ToString();
        //add in a 0 infront of single digit seconds
        if (fixedSeconds.Length == 1){
            fixedSeconds = "0" + fixedSeconds;
        }
        string timeinString = minutes.ToString() + ":" + fixedSeconds;
        return timeinString;
    }

}
