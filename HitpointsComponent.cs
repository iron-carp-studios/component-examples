using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using IronCarpStudios.ARPG.Data;
using IronCarpStudios.ARPG.UI;
using UnityEngine;

namespace IronCarpStudios.ARPG.Components.Actions
{
    public class HitpointsComponent : AgentComponent
    {
        public AgentStats agentStats;
        public bool InvulnerabilityEnabled;

        public override void OnEnable()
        {

            base.OnEnable();
            agentStats = GetComponent<AgentStats>();
        }

        public override void OnDisable()
        {
            agentStats = null;
            base.OnDisable();
        }

        protected override void Subscribe()
        {
            
            base.Subscribe();
            agent.AddListener(new EventRegistrationData("TakeDamage", OnTakeDamage));
        }

        private void OnTakeDamage(Agent sender, AgentEventArgs args)
        {
            if (InvulnerabilityEnabled) return;

            var e = args as DamageEventArgs;

            //null damage is special, it means some other component negated the damage.
            float? damage = e?.RawDamage ?? null;
            agentStats.Stats.Health.Value = Mathf.Clamp(agentStats.Stats.Health.Value - damage ?? 0, 0, agentStats.Stats.MaxHealth.Value);

            GlobalEvent.Broadcast("UpdateHealthValueUI", this.agent, new StaminaUIEventArgs(agentStats.Stats.Health.Value, agentStats.Stats.MaxHealth.Value));
            GlobalEvent.Broadcast("UpdateStaminaValueUI", this.agent, new StaminaUIEventArgs(agentStats.Stats.Stamina.Value, agentStats.Stats.MaxHealth.Value));

        }
    }
}