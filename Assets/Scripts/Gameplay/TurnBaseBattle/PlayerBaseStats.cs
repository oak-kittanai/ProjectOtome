using UnityEngine;

namespace TurnBase
{
    public class PlayerBaseStats : Singleton<PlayerBaseStats>
    {
        [Header("Player Stats&Setting")]
        public Sprite playerImage;
        public int maxHealth;
        public int currentHealth;
        public int defend;
        public int playerDamage;

        [Header("Animation")]
        public Animator animator;

        void Start()
        {
            currentHealth = maxHealth;
        }

        void Update()
        {

        }

        public void ReceiveDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth == 0)
            {

            }
            else
            {

            }
        }
    }
}

