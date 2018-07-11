using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace JulyJam.Core{
    public class GameController : MonoBehaviour{
        [SerializeField] private ShipHandler _ship;
        [SerializeField] private PlayerController _player;
        public GameObject endScreenUI;

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

        }
    }
}