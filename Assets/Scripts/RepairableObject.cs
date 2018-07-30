using System;
using System.Collections.Generic;
using System.Text;
using JulyJam.Player;
using JulyJam.UI;
using MovementEffects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JulyJam.Interactables{
    /// <summary>
    /// Class to define an object that breaks and is repairable
    /// </summary>
    public class RepairableObject : Interactable{
        [SerializeField] private float _malfunctionInterval; //how often we roll a check
        [SerializeField] private float _malfunctionWeight; //whats the chance we break on a check out of 100
        [SerializeField] private float _partHealthDrain = 1f; //how much health does this part drain when damaged
        [SerializeField] private float _irrepairableTime = 30f; //time it takes to permanently damage the ship
        [SerializeField] private float _totalShipDamage = 0f; //how much damage to the ship does this do when destroyed?
        [SerializeField] private float _shipRecoverValue = 20f; //how much does the ship heal by when repaired?
        [SerializeField] private bool _isBroken;
        [SerializeField] private float _errorDamageValue = 5f; //how much time is deducted when the player makes a mistake
        private bool _isDestroyed;
        private float _repairRateRamping = 7.5f; //For every successful pass on the break roll, increase the % by this amount
        private float _modifiedMalfunctionWt; //the current rate of failure after modification by the ramping;

        public int scoreValue = 25; //the amount of score you recieve per successful repair

        public Healthbar irrepairableTimerUI;

        //a part is destroyed, remove health
        public delegate void PartDestroyedEvent(float damage, float originalDrain, RepairableObject rObject);
        public PartDestroyedEvent PartDestroyed;

        //a part is broken, add drain to ship
        public delegate void PartBrokenEvent(float drain);
        public PartBrokenEvent PartBroken;

        //a part is repaired, remove drain from ship
        public delegate void PartRepairedEvent(float drain);
        public PartRepairedEvent PartRepaired;

        //a part was partially repaired, heal the ship a little bit and add to the score
        public delegate void ShipHealedEvent(float value, int score);
        public ShipHealedEvent HealShip;

        public GameObject needsRepairMarker;
        public GameObject isDestroyedMarker;
        public GameObject inputIndicatorArrow;

        private float _timeSinceLastCheck = 0f;
        private float _destroyTimer = 0f; //time until the part is destroyed

        [Range(1, 4)] public int difficulty;
        private Stack<char> _solutionStack; //using a stack to store our solution, because input order matters
        private readonly char[] _validKeys = new[]{'H', 'J', 'K', 'L'}; //all valid key presses

        private PlayerMovement _player;

        public RepairableInputUI repairUI;

        void Start(){
            _modifiedMalfunctionWt = _malfunctionWeight; //start off at baseline failure rate
            needsRepairMarker.SetActive(false);
            _solutionStack = new Stack<char>(Mathf.CeilToInt(difficulty)); //set the max cap of our stack to the difficulty
        }

        // Update is called once per frame
        void Update(){
            if (!_isBroken && !_isDestroyed){
                //if we aren't broken
                _timeSinceLastCheck += Time.deltaTime;
                if (_timeSinceLastCheck >= Mathf.FloorToInt(_malfunctionInterval)){
                    //check if its time to roll
                    int rng = Random.Range(0, 101);
                    if (rng < _modifiedMalfunctionWt) {
                        //do the roll and check
                        BreakObject(); //we broke
                        _modifiedMalfunctionWt = _malfunctionWeight; //reset the modified weight after a successful break
                    }
                    else{
                        _modifiedMalfunctionWt += _repairRateRamping; // ramp the failure rate up everytime we don't break
                    }
                    _timeSinceLastCheck = 0;
                }
            }
            //tick down the repair timer
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

            CreateSolutionStack(); //generate a new solution when broken

            PartBroken(_partHealthDrain);
            needsRepairMarker.SetActive(true);
            irrepairableTimerUI.gameObject.SetActive(true);
            //start counting draining health until
            //do some fx and sprite swaps in here
        }

        /// <summary>
        /// create a randomized stack of keys we need to press for this room
        /// </summary>
        private void CreateSolutionStack(){
            StringBuilder sb = new StringBuilder((int)difficulty); //for the ui
            //run through our difficulty counter and create a random stack of input
            for (int i = 0; i < difficulty; i++){
                int index = Random.Range(0, 4);
                _solutionStack.Push(_validKeys[index]);

                //append input and direction of input
                sb.Append(_validKeys[index]);
                if (i != (int)difficulty - 1){
                    sb.Append('>');
                }
            }
            //reverse the string to get the proper order of input
            //inputs are added in FILO, so right to left is what they are being removed as, except human beans read left to right so we do this
            char[] s = sb.ToString().ToCharArray();
            Array.Reverse(s);
            string temp = new string(s);
            repairUI.DisplayInputString(temp);
            //inputDirectorField.text = new string(s);
            //inputDirectorField.gameObject.SetActive(true);
            inputIndicatorArrow.transform.localPosition = new Vector3(-2.75f, 0.5f, -1.5f); //reset arrow to homeposition
        }

        /// <summary>
        /// called to make this part permanently broken
        /// </summary>
        public void DestroyObject(){ 
            //break this part and make it non interactable
            _isDestroyed = true;
            _isInteractable = false;
            //turn of all the ui
            needsRepairMarker.SetActive(false);
            irrepairableTimerUI.gameObject.SetActive(false);
            inputIndicatorArrow.SetActive(false);
            repairUI.DisableAll();
            isDestroyedMarker.SetActive(true);
            //send the event we broke
            PartDestroyed(_totalShipDamage, _partHealthDrain, this);
        }

        /// <summary>
        /// caled to fix the object
        /// </summary>
        public void RepairObject(){
            needsRepairMarker.SetActive(false);
            irrepairableTimerUI.gameObject.SetActive(false);
            repairUI.DisableAll();
            PartRepaired(_partHealthDrain);
            _isInteractable = true;
            _isBroken = false;
        }

        /// <summary>
        /// initiate repairs with the primary interact key
        /// </summary>
        /// <param name="player"></param>
        public override void Interact(PlayerMovement player){
            if (_isInteractable && _isBroken){
                _player = player;
                _isInteractable = false;
                _player.isInteracting = true;
                inputIndicatorArrow.SetActive(true);
                //Timing.RunCoroutine(DoRepair(player));
            }
        }

        //TODO: maybe have this do a return value
        /// <summary>
        /// Accept individual key inputs for the repair minigame
        /// </summary>
        /// <param name="key"></param>
        public override bool Interact(char key){
            bool isRepaired = false;
            if (_player != null && _player.isInteracting){
                //check if the current key is what we pressed
                if (_solutionStack.Peek() == key){
                    //visual hooks here
                    //shift prism on X + 125 per letter completed
                    inputIndicatorArrow.transform.localPosition += new Vector3(2.5f, 0, 0);
                    //if so pop
                    _solutionStack.Pop();
                    HealShip(_shipRecoverValue, scoreValue); //heal the ship a little since we hit a key successfully
                    isRepaired = true;
                }
                else{
                    _destroyTimer += _errorDamageValue;
                    if (irrepairableTimerUI) {
                        irrepairableTimerUI.UpdateHealthBar((_irrepairableTime - _destroyTimer) / _irrepairableTime);
                    }
                    if (_destroyTimer >= _irrepairableTime) {
                        _player.isInteracting = false;
                        _player = null;
                        DestroyObject();
                    }
                }
                //check for when we are done
                if (_solutionStack.Count == 0){
                    RepairObject();
                    _player.isInteracting = false;
                    _player = null;
                    inputIndicatorArrow.SetActive(false);
                }
                return isRepaired;
            }
            return false; //this should never be returned
        }

        /// <summary>
        /// the act of repairing
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private IEnumerator<float> DoRepair(PlayerMovement player){
            yield return Timing.WaitForSeconds(timeToInteract);
            player.isInteracting = false;
            RepairObject();
            yield return 0f;
        }
    }
}