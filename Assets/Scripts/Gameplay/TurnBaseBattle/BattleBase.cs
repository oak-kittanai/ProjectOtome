using UnityEngine;

namespace TurnBase
{
    public class BattleBase : Singleton<BattleBase>
    {
        [Header("TB Setting")]
        public int Stats;

        [Header("Player Setting")]
        public GameObject currentSelectedEnemy;

        [Header("Enemy Setting")]
        public GameObject[] selectedEnemy;
        EnemyStats enemyStats;

        public void PlayerAttack(int damage)
        {

        }
    }

    public class EnemyStats
    {
        public Sprite enemyImage;
        public int enemyDamage;
        public int enemyHp;
    }
}
