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
        public float detectionRadius = 50;
        public LayerMask detectionLayer;
        public TextMeshProUGUI text;
        public GameObject door;
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

        private void Start()
        {
            player.SetActive(false);
            startMenuUI.SetActive(true);
            startGameBTN.onClick.AddListener(StartGame);
            quitGameBTN.onClick.AddListener(QuitGame);
            nextLevelBTN.onClick.AddListener(CreditScreen);
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
            }
        }
        public void LevelComplete() 
        {
            player.SetActive(false);
            gameUI.SetActive(false);
            levelCompleteUI.SetActive(true);
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
        }
        public void QuitGame() 
        {
            Application.Quit();
        }
    }
}
