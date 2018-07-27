using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MovementEffects;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle each high score entry
/// </summary>
[Serializable]
public class LeaderBoardEntry{
    public string levelName;
    public int timeSpent;
    public int finalScore;

    public LeaderBoardEntry(string level, int time, int score){
        levelName = level;
        timeSpent = time;
        finalScore = score;
    }
}

/// <summary>
/// Class to handle the leader boards for the game
/// </summary>
public class LocalLeaderboardsHandler : MonoBehaviour{
    public static LocalLeaderboardsHandler Instance = null;
    public List<LeaderBoardEntry> leaderBoards = new List<LeaderBoardEntry>(10);

    private const string SAVE_DATA_FILENAME_BASE = "savedata";
    private const string SAVE_DATA_EXTENSION = ".json";
    private string SAVA_DATA_PATH = Application.persistentDataPath + "\\leaderBoardData\\";

    // Use this for initialization
    void Awake (){
        //this currently works
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// check the new score against the board
    /// </summary>
    /// <param name="newEntry"></param>
    public void CheckAgainstLeaderBoard(LeaderBoardEntry newEntry){
        //empty score list, so just add it in
        if (leaderBoards.Count == 0){
            leaderBoards.Add(newEntry);
            return;
        }

        //for future refernce, building bottom  up seems like a bad idea
        for(int i = 0; i < leaderBoards.Count; i++) {
            //current score is less than a value on the board, so insert it under that score
            if (leaderBoards[i].finalScore < newEntry.finalScore){
                leaderBoards.Insert(i, newEntry);
                break;
            }
            //if the scores are the same, check by time survived
            if (leaderBoards[i].finalScore == newEntry.finalScore){
                //compare by time Longer is better
                // then do a replace
                if (leaderBoards[i].timeSpent < newEntry.timeSpent){
                    //the new score has a higher time, so put it in the normal place above the score
                    leaderBoards.Insert(i, newEntry);
                }
                else{
                    //the new time is lower, so put keep the existing score in its place
                    leaderBoards.Insert(i++, newEntry);
                }
                break;
            }
        }
    }

    public void SaveScoresToFile(LeaderBoardEntry entry){
        string json = JsonUtility.ToJson(leaderBoards);
        File.WriteAllText(json, SAVA_DATA_PATH + SAVE_DATA_FILENAME_BASE + SAVE_DATA_EXTENSION);
    }

}
