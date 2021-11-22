using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField]
        private HumanoidDefinition definition;

        private bool canRecharge;
        private int staminaRechargeRate, staminaRechargeDelay;
        public float MaxStamina
        {
            get;
            set;
        }
        public float CurrentStamina
        {
            get;
            set;
        }
        void Start()
        {
            canRecharge = true;
            MaxStamina = definition.baseStamina.BaseValue;
            CurrentStamina = MaxStamina;

            staminaRechargeRate = definition.staminaRechargeRate.BaseValue;
            staminaRechargeDelay = definition.staminaRechargeDelay.BaseValue;
        }

        private void Update()
        {
            if (CurrentStamina <= 0 && canRecharge)
            {
                canRecharge = false;
                StartCoroutine(DelayStaminaRoutine());
            }
            else if ((CurrentStamina < MaxStamina) && (CurrentStamina > 0))
            {
                CurrentStamina += staminaRechargeRate * Time.deltaTime;
            }
            CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina);
        }

        private IEnumerator DelayStaminaRoutine()
        {
            CurrentStamina = 0;
            yield return new WaitForSeconds(staminaRechargeDelay);
            canRecharge = true;
            CurrentStamina += 1;
        }
    }
}