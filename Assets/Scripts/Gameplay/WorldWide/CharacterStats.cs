using System.Collections;
using UnityEngine;

namespace FreeWorld
{
    public class CharacterStats : Singleton<CharacterStats>
    {
        public bool useItem; // for test in recharge
        public bool currentInAction;

        void Start()
        {
            currentFood = maxFood; currentStamina = maxStamina;
        }

        private void Update()
        {
            CheckOverflowStats();
            RechargingStation();

        }

        void CheckOverflowStats()
        {
            if (currentFood > maxFood)
            {
                currentFood = maxFood;
            }
            if (currentWater <= 0)
            {
                currentFood = 0;
            }

            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                staminaMax = true;
                isRecharging = false;
            }
            else
            {
                staminaMax = false;
            }

            if (currentStamina <= 0)
            {
                isExhausted = true;
                currentStamina = 0;
            }
        }

        #region Food&WaterZone
        public float currentFood;
        public float maxFood;
        public float currentWater;
        public float maxWater;

        public void EatingItem(float calorie, string type)
        {
            if (type == "Food")
            {
                currentFood += calorie;
            }
        }

        #endregion

        #region StaminaZone
        public float currentStamina;
        public float maxStamina;
        public bool rechargeStamina;
        public bool isRecharging;
        public bool staminaMax;

        public bool isExhausted;

        void RechargingStation()
        {
            if (isExhausted)
            {
                if (currentStamina <= 45f)
                {
                    isExhausted = false;
                }
            }

            if (rechargeStamina && !currentInAction)
            {
                StartCoroutine(StartRechargeing());
                rechargeStamina = false;
            }

            if (isRecharging)
            {
                RechargeStamina(1);
            }

            if (!staminaMax && !currentInAction)
            {
                isRecharging = true;
            }else
            {
                isRecharging = false;
            }
        }

        IEnumerator StartRechargeing()
        {
            isRecharging = true;
            yield return new WaitForSeconds(2f);
        }

        public void UseStamina(float stamina, bool stillAction) 
        {
            currentInAction = stillAction;

            if (stamina <= 0)
            {

            }
            else
            {
                currentStamina -= stamina * Time.deltaTime;
                rechargeStamina = false;
            }
            
        }

        public void RechargeStamina(float stamina)
        {
            if (useItem)
            {
                // use item to recharge

                return;
            }

            // Nomal Recharge

            if (stamina == 1)
            {
                currentStamina += 2 * Time.deltaTime;

                if (staminaMax)
                {
                    isRecharging = false;
                }
            }
        }

        #endregion
    }
}

