
using UnityEngine;
using UnityEngine.UI;

namespace JulyJam.UI{
    public class Healthbar : MonoBehaviour{
        public Image healthBar;
        public Image maxHealthBar;
        private float _healthRatioAfterWound = 1f;

        public void UpdateHealthBar(float hp){
            //Scales the foreground of the healthbar to the value of hp clamped between 0 and 1
            /*healthBar.transform.localScale = new Vector3(Mathf.Clamp(hp, 0f, 1f), healthBar.transform.localScale.y,
                healthBar.transform.localScale.z);*/
            healthBar.fillAmount = hp;
        }

        /// <summary>
        /// Used to update the current max health from the absolute max
        /// </summary>
        /// <param name="hp"></param>
        public void UpdateMaxBar(float currentHp, float currentMax, float absoluteMax){
            _healthRatioAfterWound = currentMax / absoluteMax;
            maxHealthBar.fillAmount = _healthRatioAfterWound;
            healthBar.fillAmount = (currentHp / currentMax) * _healthRatioAfterWound;
        }
    }
}