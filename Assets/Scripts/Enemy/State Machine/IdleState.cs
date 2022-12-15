using mohib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class IdleState : State
    {
        public LayerMask detectionLayer;
        public ChaseState chaseState;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
                if (characterStats != null)
                {
                    if (characterStats.tag == "Player")
                    {
                        Vector3 targetDirection = characterStats.transform.position - transform.position;
                        float viewAbleAngle = Vector3.Angle(targetDirection, transform.forward);
                        if (viewAbleAngle > enemyManager.minimumDetectionAngle && viewAbleAngle < enemyManager.maximumDetectionAngle)
                        {
                            enemyManager.currentTarget = characterStats;
                            
                        }
                    }
                }
            }
            if (enemyManager.currentTarget != null)
            {
                return chaseState;
            }
            else 
            {
                return this;
            }
        }
    }
}

