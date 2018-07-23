using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JulyJam.Player;
using JulyJam.UI;
using MovementEffects;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace JulyJam.Interactables{
    /// <summary>
    /// Class to define an object that breaks and is repairable
    /// </summary>
    public class RepairableObject : Interactable{
        [SerializeField] private float _malfunctionInterval; //how often we roll a check
        [SerializeField] private float _malfunctionWeight; //whats the chance we break on a check out of 100
        public float parthHealth = 100f; //how much hp this part has
        [SerializeField] private float _partHealthDrain = 1f; //how much health does this part drain when damaged
        [SerializeField] private float _irrepairableTime = 30f; //time it takes to permanently damage the ship
        [SerializeField] private float _totalShipDamage = 0f; //how much damage to the ship does this do when destroyed?
        [SerializeField] private float _shipRecoverValue = 20f; //how much does the ship heal by when repaired?
        [SerializeField] private bool _isBroken;
        [SerializeField] private float _errorDamageValue = 5f; //how much time is deducted when the player makes a mistake
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

        //a part was partially repaired, heal the ship a little bit
        public delegate void ShipHealedEvent(float value);
        public ShipHealedEvent HealShip;

        public GameObject needsRepairMarker;
        public GameObject isDestroyedMarker;
        public Text inputDirectorField;
        public GameObject inputIndicatorArrow;

        private float _timeSinceLastCheck = 0f;
        private float _destroyTimer = 0f; //time until the part is destroyed

        [Range(1, 4)] public int difficulty;
        private Stack<char> _solutionStack; //using a stack to store our solution, because input order matters
        private readonly char[] _validKeys = new[]{'H', 'J', 'K', 'L'}; //all valid key presses

        private PlayerMovement _player;

        void Start(){
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
                    if (rng < _malfunctionWeight){
                        //do the roll and check
                        BreakObject(); //we broke
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
            inputDirectorField.text = new string(s);
            inputDirectorField.gameObject.SetActive(true);
            inputIndicatorArrow.transform.localPosition = new Vector3(35f,50f,0); //reset arrow to homeposition
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
            inputDirectorField.gameObject.SetActive(false);
            PartDestroyed(_totalShipDamage, this);
        }

        /// <summary>
        /// caled to fix the object
        /// </summary>
        public void RepairObject(){
            needsRepairMarker.SetActive(false);
            irrepairableTimerUI.gameObject.SetActive(false);
            inputDirectorField.gameObject.SetActive(false);
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

        /// <summary>
        /// Accept individual key inputs for the repair minigame
        /// </summary>
        /// <param name="key"></param>
        public override void Interact(char key){
            if (_player != null && _player.isInteracting){
                //check if the current key is what we pressed
                if (_solutionStack.Peek() == key){
                    //visual hooks here
                    //shift prism on X + 125 per letter completed
                    inputIndicatorArrow.transform.localPosition += new Vector3(125f, 0, 0);
                    //if so pop
                    _solutionStack.Pop();
                    HealShip(_shipRecoverValue); //heal the ship a little since we hit a key successfully
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
            }
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