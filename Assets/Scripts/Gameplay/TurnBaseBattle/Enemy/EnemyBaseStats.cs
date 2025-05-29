using System.Collections;
using UnityEngine;

namespace TurnBase
{
    public class EnemyBaseStats : MonoBehaviour
    {
        [Header("Enemy Setting")]
        public string enemyName;
        public Sprite enemyImage;
        public int enemyDamage;
        public int enemyMaxHp;
        public int enemyCurrentHp;

        [Header("Animation")]
        public Animator enemyAnimator;

        public void Start()
        {
            enemyCurrentHp = enemyMaxHp;
            enemyAnimator = GetComponent<Animator>();
            ResetAnimate();
        }

        public void AttackAnimate()
        {
            enemyAnimator.SetBool("isIdle", false);
            enemyAnimator.SetTrigger("isAttack");
            StartCoroutine(ResetAnimate());
        }

        public void ReceiveDamage(int damage)
        {
            Debug.Log("Dmg Receive");
            enemyCurrentHp -= damage;

            if (enemyCurrentHp <= 0)
            {
                enemyAnimator.SetBool("isDead", true);
                enemyAnimator.SetBool("isIdle", false);
            }
            else
            {
                Debug.Log("Enemy Get Hit");
                enemyAnimator.SetBool("isIdle", false);
                enemyAnimator.SetTrigger("isGetHit");
                StartCoroutine(ResetAnimate());
            }
        }

        IEnumerator ResetAnimate()
        {
            yield return new WaitForSeconds(1f);
            enemyAnimator.SetBool("isIdle", true);
            enemyAnimator.SetBool("isDead", false);
        }
    }


}

