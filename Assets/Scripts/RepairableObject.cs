using System.Collections;
using System.Collections.Generic;
using JulyJam.Player;
using JulyJam.UI;
using MovementEffects;
using UnityEngine;

namespace JulyJam.Interactables{
    /// <summary>
    /// Class to define an object that breaks and is repairable
    /// </summary>
    public class RepairableObject : Interactable{
        [SerializeField] private float _malfunctionInterval; //how often we roll a check
        [SerializeField] private float _malfunctionWeight; //whats the chance we break on a check out of 100
        public float parthHealth = 100f; //how much hp this part has
        [SerializeField] private float _partHealthDrain = 1f; //how much health does this part drain when damaged
        [SerializeField] private float _irrepairableTime = 90f; //time it takes to permanently damage the ship
        [SerializeField] private float _totalShipDamage = 0f; //how much damage to the ship does this do when destroyed?
        [SerializeField] private bool _isBroken;
        private bool _isDestroyed;

        public Healthbar irrepairableTimerUI;

        //a part is destroyed, remove health
        public delegate void PartDestroyedEvent(float damage, RepairableObject rObject);

        public PartDestroyedEvent PartDestroyed;

        //a part is broken, add drain to ship
        public delegate void PartBrokenEvent(float drain);

        public PartBrokenEvent PartBroken;

        //a part is repaired, remove drain from ship
        public delegate void PartRepairedEvent(float drain);

        public PartRepairedEvent PartRepaired;

        public GameObject needsRepairMarker;
        public GameObject isDestroyedMarker;
        private float _timeSinceLastCheck = 0f;
        private float _destroyTimer = 0f; //time until the part is destroyed

        void Start(){
            needsRepairMarker.SetActive(false);
        }

        // Update is called once per frame
        void Update(){
            if (!_isBroken && !_isDestroyed){
                //if we aren't broken
                _timeSinceLastCheck += Time.deltaTime;
                if (_timeSinceLastCheck >= Mathf.FloorToInt(_malfunctionInterval)){
                    //check if its time to roll
                    int rng = Random.Range(0, 101);
                    if (rng < _malfunctionWeight){
                        //do the roll and check
                        BreakObject(); //we broke
                    }
                    _timeSinceLastCheck = 0;
                }
            }
            if (_isBroken && !_isDestroyed && _isInteractable){
                _destroyTimer += Time.deltaTime;
                if (irrepairableTimerUI){
                    irrepairableTimerUI.UpdateHealthBar((_irrepairableTime - _destroyTimer) / _irrepairableTime);
                }
                if (_destroyTimer >= _irrepairableTime){
                    DestroyObject();
                }
            }
        }

        /// <summary>
        /// called to break the object
        /// </summary>
        public void BreakObject(){
            _isBroken = true;
            _destroyTimer = 0f;
            PartBroken(_partHealthDrain);
            needsRepairMarker.SetActive(true);
            irrepairableTimerUI.gameObject.SetActive(true);
            //start counting draining health until
            //do some fx and sprite swaps in here
        }

        /// <summary>
        /// called to make this part permanently broken
        /// </summary>
        public void DestroyObject(){
            _isDestroyed = true;
            _isInteractable = false;
            needsRepairMarker.SetActive(false);
            irrepairableTimerUI.gameObject.SetActive(false);
            isDestroyedMarker.SetActive(true);
            PartDestroyed(_totalShipDamage, this);
        }

        /// <summary>
        /// caled to fix the object
        /// </summary>
        public void RepairObject(){
            needsRepairMarker.SetActive(false);
            irrepairableTimerUI.gameObject.SetActive(false);
            PartRepaired(_partHealthDrain);
            _isBroken = false;
        }

        /// <summary>
        /// initiate repairs
        /// </summary>
        /// <param name="player"></param>
        public override void Interact(PlayerMovement player){
            if (_isInteractable && _isBroken){
                _isInteractable = false;
                player.isInteracting = true;
                Timing.RunCoroutine(DoRepair(player));
            }
        }

        /// <summary>
        /// the act of repairing
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private IEnumerator<float> DoRepair(PlayerMovement player){
            yield return Timing.WaitForSeconds(timeToInteract);
            _isInteractable = true;
            player.isInteracting = false;
            RepairObject();
            yield return 0f;
        }
    }
}