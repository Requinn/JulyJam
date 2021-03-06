﻿using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JLProject {
    /// <summary>
    /// Static referenced sceneloader
    /// </summary>
    public class SceneLoader : MonoBehaviour {
        public static SceneLoader Instance;
        public delegate void StartLoadEvent(float duration);
        public event StartLoadEvent OnStartLoad;
        public delegate void EndLoadEvent(float duration);
        public event EndLoadEvent OnEndLoad;

        public float minimumWaitTime = 1f;
        public float postReadyDelay = 1f;
        private bool loadScene = false;

        private void Awake() {
            Instance = this;
        }

        /// <summary>
        /// autosave and go to the next level
        /// </summary>
        public void LoadNextLevel() {
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            LoadLevel(nextScene);
        }

        /// <summary>
        /// go to the specified level
        /// </summary>
        /// <param name="sceneIndex"></param>
        public void LoadLevel(int sceneIndex = 1) {
            if (!loadScene) {
                loadScene = true;
                /**
                //if we have a player stats to update, do so. If not it will be autogenerated on next level load
                if (DataService.Instance.PlayerStats != null){
                    //this is solving the issue of loading a scene selected from the menu
                    //having a player load into a level that is not directly next from within the level will cause an error
                    PlayerController _PC = FindObjectOfType<PlayerController>(); 
                    if (_PC){
                        DataService.Instance.PlayerStats.UpdateStats(_PC, sceneIndex);
                    }
                }**/
                OnStartLoad(minimumWaitTime); //fire off an event for a loading screen or something
                //handle a main menu call real quick
                if (sceneIndex == 0){
                    Time.timeScale = 1f;
                }
                Timing.RunCoroutine(CoLoadScene(sceneIndex));

            }
        }

        /// <summary>
        /// Load a scene in the coroutine to track progress
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private IEnumerator<float> CoLoadScene(int index) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index); //load scene async
            asyncLoad.allowSceneActivation = false;

            yield return Timing.WaitForSeconds(minimumWaitTime);

            while (asyncLoad.progress < 0.89f) {
                yield return 0f; //spinlock my dudes
            }

            //OnEndLoad(postReadyDelay);
            yield return Timing.WaitForSeconds(postReadyDelay);
            asyncLoad.allowSceneActivation = true;
        }

        /// <summary>
        /// Hi this shouldn't be here but we need it now just a little
        /// </summary>
        public void QuitGame(){
            Application.Quit();
        }
    }
}