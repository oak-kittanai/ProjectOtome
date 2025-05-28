using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBase
{
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

    public class BattleBaseSystem : Singleton<BattleBaseSystem>
    {
        [Header("TurnBase")]
        public BattleState battleState;

        [Header("Player Setting")]
        public GameObject playerPrefab;
        public Transform playerBattlePosition;
        PlayerBaseStats playerStats;

        public int currentSelectedEnemy;
        private bool changeTarget = true;

        public GameObject CurrentGameObject;

        [Header("Enemy Setting")] // Add the current Enemy to Start
        public int enemiesNumber;
        public int enemiesIndex;
        public GameObject[] enemyPrefab;
        public Transform[] enemyBattlePosition;

        public List<EnemyData> enemyStatsList = new List<EnemyData>();

        public int enemyCurrentTurn;
        public string StatusReturn = null;

        private bool firstimeStart = true;

        public void ForcePlayBattleBase(int enemiesNumber)
        {
            battleState = BattleState.START;
            SetupBattle(enemiesNumber);
            firstimeStart = true;
        }

        private void Update()
        {
            if (battleState == BattleState.PLAYERTURN)
            {
                SelecetEnemy();
            }
        }

        public void SetupEnemy(int number)
        {
            enemiesIndex = 0;
            int RandomIndex;
            int PositionIndex = 0;
            for (int i = 0; i < number; i++)
            {
                RandomIndex = Random.Range(0, enemyPrefab.Length);
                GameObject enemySet = Instantiate(enemyPrefab[RandomIndex], enemyBattlePosition[PositionIndex]);
                EnemyBaseStats enemyStats = enemySet.GetComponent<EnemyBaseStats>();
                enemyStatsList.Add(new EnemyData(enemySet, enemyStats, enemiesIndex));
                enemiesIndex++;
                PositionIndex++;
            }

        }

        void SetupBattle(int enemyNum)
        {
            GameObject playerSet = Instantiate(playerPrefab, playerBattlePosition);
            playerStats = playerSet.GetComponent<PlayerBaseStats>();

            Debug.Log("SetupStart");

            SetupEnemy(enemyNum);

            // Add to the HUD



            // 

            

            battleState = BattleState.PLAYERTURN;
            PlayerTurn();

        }

        void SelecetEnemy()
        {
            if (currentSelectedEnemy >= enemyStatsList.Count)
            {
                currentSelectedEnemy = enemyStatsList.Count - 2;
            }

            if (currentSelectedEnemy < 0)
            {
                currentSelectedEnemy = 0;
            }

            if (changeTarget && battleState == BattleState.PLAYERTURN)
            {
                ShowMarkedEnemy();
            }
            GetInput();
        }

        void ShowMarkedEnemy()
        {
            for (int i = 0; i < enemyStatsList.Count; i++)
            {
                if (enemyStatsList[i].enemyNum == currentSelectedEnemy)
                {
                    //GameObject Marked = Instantiate(); Add Mark 
                    CurrentGameObject = enemyStatsList[i].enemyGO;
                    Debug.Log("Change Tartget to : " + CurrentGameObject + " : " + enemyStatsList[i].enemyNum);
                    // Add Hud

                    //
                    changeTarget = false;
                    break;
                }
            }

        }

        void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentSelectedEnemy++;
                changeTarget = true;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentSelectedEnemy--;
                changeTarget = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject clickedEnemy = hit.collider.gameObject;
                    if (clickedEnemy != null)
                    {
                        for (int i = 0; i < enemyStatsList.Count; i++)
                        {
                            if (enemyStatsList[i].enemyGO == clickedEnemy)
                            {
                                int enemiesIndex = enemyStatsList[i].enemyNum;
                                currentSelectedEnemy = enemiesIndex;
                                changeTarget = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        void PlayerTurn()
        {
            Debug.Log("PlayerTurn");

        }

        void EnemyTurn()
        {
            ChangeEnemyTurn();

            Debug.Log("EnemyTurn");
        }

        void ChangeEnemyTurn()
        {
            enemyCurrentTurn++;

            if (enemyCurrentTurn >= 4)
            {
                enemyCurrentTurn = 0;
            }

            if (firstimeStart)
            {
                enemyCurrentTurn = 0;
                firstimeStart = false;
            }
        }

        public void StartAttack()
        {
            EnemyBaseStats enemyToAttack = enemyStatsList[currentSelectedEnemy].stats;

            StartCoroutine(Attack(enemyToAttack, playerStats));
        }

        IEnumerator Attack(EnemyBaseStats enemy, PlayerBaseStats player)
        {
            if (battleState == BattleState.PLAYERTURN)
            {
                enemy.ReceiveDamage(playerStats.playerDamage);
                yield return new WaitForSeconds(2f);
                battleState = BattleState.ENEMYTURN;
                EnemyTurn();
            }
            else
            {
                BattleBaseManager.Instance.ForceQTEPlay();
                Debug.Log("Enemy Attack");
                enemy.AttackAnimate();

                while (string.IsNullOrEmpty(StatusReturn)) yield return null;

                Debug.Log("WaitUntil Broke");
                Debug.Log(StatusReturn);

                switch (StatusReturn)
                {
                    case "Dodge":
                        player.ReceiveDamage(0);
                        print("Player Dodge");
                        break;

                    case "Def":
                        int realdamage = enemy.enemyDamage - player.defend;
                        player.ReceiveDamage(realdamage);
                        print("Player Take " + realdamage);
                        break;

                    case "Dmg":
                        player.ReceiveDamage(enemy.enemyDamage);
                        print("Player Take " + enemy.enemyDamage);
                        break;

                    default:

                        break;
                }

                StatusReturn = "";

                yield return new WaitForSeconds(2f);
                battleState = BattleState.PLAYERTURN;
                StatusReturn = null;
                PlayerTurn();
            }
        }
    }
    
    public class EnemyData
    {
        public int enemyNum;
        public GameObject enemyGO;
        public EnemyBaseStats stats;

        public EnemyData(GameObject enemygo, EnemyBaseStats s, int enemyNum)
        {
            enemyGO = enemygo;
            stats = s;
            this.enemyNum = enemyNum;
        }
    }
}
