using UnityEngine;
using UnityEngine.UI;

namespace TurnBase
{
    public class BattleBaseHUD : MonoBehaviour
    {
        public Button playerAttackButton, enemyAttackButton;

        public bool playerTurn = false;
        public bool enemyTurn = false;

        private void Start()
        {
            playerAttackButton.onClick.AddListener(BattleBaseSystem.Instance.StartAttack);
            enemyAttackButton.onClick.AddListener(BattleBaseSystem.Instance.StartAttack);
        }

        private void Update()
        {

            if (BattleBaseSystem.Instance.battleState == BattleState.START)
            {
                playerAttackButton.gameObject.SetActive(false);
                enemyAttackButton.gameObject.SetActive(false);
            }

            if (BattleBaseSystem.Instance.battleState == BattleState.PLAYERTURN) { playerTurn = true; enemyTurn = false; }

            if (BattleBaseSystem.Instance.battleState == BattleState.ENEMYTURN) { enemyTurn = true; playerTurn = true; }

            if (playerTurn)
            {
                playerAttackButton.gameObject.SetActive(true);
                enemyAttackButton.gameObject.SetActive(false);
            }
            

            if (enemyTurn)
            {
                playerAttackButton.gameObject.SetActive(false);
                enemyAttackButton.gameObject.SetActive(true);
            }



            if (BattleBaseSystem.Instance.battleState == BattleState.PLAYERTURN)
            {
                playerAttackButton.enabled = true;
            }
            else
            {
                playerAttackButton.enabled = false;
            }

            if (BattleBaseSystem.Instance.battleState == BattleState.ENEMYTURN)
            {
                enemyAttackButton.enabled = true;
            }
            else
            {
                enemyAttackButton.enabled = false;
            }
        }
    }


}

