using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace Mohib 
{
    public class GameManager : MonoBehaviour
    {
        public int enemyCount;
        public int levelCount = 0;
        public float detectionRadius = 50;
        public LayerMask detectionLayer;
        public TextMeshProUGUI text;
        public GameObject door;
        public GameObject levelDoor;
        public Button startGameBTN;
        public Button quitGameBTN;
        public GameObject startMenuUI;
        public GameObject gameUI;
        public GameObject levelCompleteUI;
        public Button nextLevelBTN;
        public Button endGameBTN;
        public GameObject creditScreen;
        public Button creditQuitBTN;
        public GameObject player;
        public GameObject nextLevel;

        private void Start()
        {
            player.SetActive(false);
            startMenuUI.SetActive(true);
            startGameBTN.onClick.AddListener(StartGame);
            quitGameBTN.onClick.AddListener(QuitGame);
            nextLevelBTN.onClick.AddListener(NextLevel);
            endGameBTN.onClick.AddListener(QuitGame);
            creditQuitBTN.onClick.AddListener(QuitGame);
            FindAllEnemies();
        }
        private void LateUpdate()
        {
            setEnemyCount();
            NextStage();
        }
        private void FindAllEnemies()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Enemy") 
                {
                    enemyCount++;
                }
            }
        }
        private void setEnemyCount() 
        {
            text.text = "Enemies Left : " + enemyCount;
        }

        public void GameOver() 
        {

        }
        public void NextStage() 
        {
            if (enemyCount == 0)
            {
                door.SetActive(false);
                levelDoor.SetActive(false);
            }
            else 
            {
                door.SetActive(true);
                levelDoor.SetActive(true);
            }
        }
        public void NextLevel() 
        {
            if (levelCount <= 1)
            {
                levelCompleteUI.SetActive(false);
                gameUI.SetActive(true);
                player.SetActive(true);
                player.GetComponent<PlayerStats>().currentHealth = player.GetComponent<PlayerStats>().maxHealth;
                nextLevel.SetActive(true);
                FindAllEnemies();
            }
            else 
            {
                CreditScreen();
            }

        }
        public void LevelComplete() 
        {
            player.SetActive(false);
            gameUI.SetActive(false);
            levelCompleteUI.SetActive(true);
            levelCount++;
        }
        public void StartGame() 
        {
            player.SetActive(true);
            startMenuUI.SetActive(false);
            gameUI.SetActive(true);
        }
        public void CreditScreen() 
        {
            creditScreen.SetActive(true);
            levelCompleteUI.SetActive(false);
            player.SetActive(false);
            gameUI.SetActive(false);
        }
        public void QuitGame() 
        {
            Application.Quit();
        }
    }
}
