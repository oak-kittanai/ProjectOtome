using UnityEngine;

namespace TurnBase
{
    public class StatsBase : Singleton<StatsBase>
    {
        [Header("Player Stats")]
        public int maxHealth;
        public int minHealth;
        public int playerDamage;

        void Start()
        {
            minHealth = maxHealth;
        }

        void Update()
        {

        }

        public void ReceiveDamage(int damage)
        {

        }
    }
}

