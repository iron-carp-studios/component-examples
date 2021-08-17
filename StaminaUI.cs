using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace IronCarpStudios.ARPG.UI
{
    public class StaminaUI : AgentComponent
    {
        public Image bar;
        public Image drainBar;
        public float animationRate;
        public bool IsActive;

        protected override void Subscribe()
        {
            IsActive = false;
            GlobalEvent.AddListener(new EventRegistrationData("UpdateStaminaValueUI", UpdateStaminaValueUI));
            bar.fillAmount = 1;
            drainBar.fillAmount = 1;
        }

        private void UpdateStaminaValueUI(Agent sender, AgentEventArgs args)
        {
            var parameters = args as StaminaUIEventArgs;
            var stamina = parameters?.CurrentValue ?? 1;
            var maxStamina = parameters?.MaxValue ?? 1;
            var fill = Mathf.Clamp(stamina / maxStamina, 0f, 1f);

            if (bar)
            {
                bar.fillAmount = fill;
                TickDrainBar();
            }
        }

        private void TickDrainBar()
        {
            if (!IsActive)
            {
                IsActive = true;
                StartCoroutine(TickDrainBarRoutine());
            }
        }

        private IEnumerator TickDrainBarRoutine()
        {
            var rate = animationRate;
            while (drainBar.fillAmount > bar.fillAmount)
            {
                rate += (Time.deltaTime/4);
                drainBar.fillAmount -= Time.deltaTime * rate
                    / 10;
                yield return new WaitForSeconds(0);
            }

            drainBar.fillAmount = bar.fillAmount;
            IsActive = false;
        }
    }

    public class StaminaUIEventArgs : AgentEventArgs
    {
        public float CurrentValue { get; set; }
        public float MaxValue { get; set; }

        public StaminaUIEventArgs(float currentValue, float maxValue)
        {
            CurrentValue = currentValue;
            MaxValue = maxValue;
        }
    }
}
