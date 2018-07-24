using System.Collections;
using System.Collections.Generic;
using JulyJam.Player;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace JulyJam.Core{
    public class GameController : MonoBehaviour{
        [SerializeField] private ShipHandler _ship;
        [SerializeField] private PlayerMovement _player;

        public GameObject endScreenUI;
        public GameObject pauseScreenUI;

        public static GameController Instance; //static ref to the game

        public delegate void GameOverEvent();
        public GameOverEvent GameOver;

        // Use this for initialization
        void Awake(){
            Instance = this;
        }

        void Start(){
            _ship.ShipDeath += HandleGameEnd;
        }
        /// <summary>
        /// handle the events that happen when we die
        /// </summary>
        private void HandleGameEnd(){
            endScreenUI.SetActive(true);
            GameOver();
        }

        /// <summary>
        /// reset the game
        /// </summary>
        public void ResetGame(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Update is called once per frame
        void Update(){
            if (Input.GetKeyDown(KeyCode.Escape)){
                if (pauseScreenUI.activeInHierarchy){
                    ClosePauseMenu();
                }
                else{
                    OpenPauseMenu();
                }
                //bring up the menu if active
                //close if not active
            }
        }

        /// <summary>
        /// stop time and open the paus menu
        /// </summary>
        public void OpenPauseMenu(){
            Time.timeScale = 0f;
            pauseScreenUI.SetActive(true);
            _player.HaltInput();
        }

        /// <summary>
        /// resume time and close the menu
        /// </summary>
        public void ClosePauseMenu(){
            Time.timeScale = 1f;
            pauseScreenUI.SetActive(false);
            _player.ResumeInput();
        }
    }
}