using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorManager enemyAnimatorManager;
        public GameManager gameManager;
        public HealthBar healthBar;

        private void Awake()
        {
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        }
        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        public void TakeDamage(int damage)
        {
            if (isDead)
            {
                Destroy(gameObject);
                gameManager.enemyCount--;
                return;
            }
            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);
            enemyAnimatorManager.PlayTargetAnimation("GetHit", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                enemyAnimatorManager.PlayTargetAnimation("Death", true);
                isDead = true;
            }
            if (isDead)
            {
                Destroy(gameObject);
                gameManager.enemyCount--;
            }
        }
    }

}
