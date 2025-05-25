using UnityEngine;

namespace TurnBase
{
    public class EnemyBaseStats : MonoBehaviour
    {
        public string enemyName;
        public Sprite enemyImage;
        public int enemyMinDamage;
        public int enemyMaxDamage;
        public int enemyHp;
        public Animator enemyAnimator;

        public void Update()
        {

        }

        public int GetRealDamage()
        {
            int realDamage = Random.Range(enemyMinDamage, enemyMaxDamage);
            return realDamage;
        }
    }

    
}

