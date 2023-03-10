using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewAbleAngle = Vector3.Angle(targetDirection, transform.forward);

            HandleRotation(enemyManager);


            if (enemyManager.isPerformingAction)
            {
                return combatStanceState;
            }
            if (currentAttack != null)
            {
                if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                {
                    return this;
                }
                else if (distanceFromTarget < currentAttack.MaximumDistanceNeededtoAttack) 
                {
                    if (viewAbleAngle <= currentAttack.maximumAttackAngle && viewAbleAngle >= currentAttack.minimumAttackAngle) 
                    {
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false) 
                        {
                            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPerformingAction = true;
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                            currentAttack = null;
                            return combatStanceState;
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(enemyManager);

            }

            return combatStanceState;
        }
        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewAbleAngle = Vector3.Angle(targetDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttack = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttack.MaximumDistanceNeededtoAttack && distanceFromTarget >= enemyAttack.minimumDistanceNeededToAttack)
                {
                    if (viewAbleAngle <= enemyAttack.maximumAttackAngle && viewAbleAngle >= enemyAttack.minimumAttackAngle)
                    {
                        maxScore += enemyAttack.attackScore;
                    }
                }
            }
            int randomValue = Random.Range(0, maxScore);
            int tempScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttack = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttack.MaximumDistanceNeededtoAttack && distanceFromTarget >= enemyAttack.minimumDistanceNeededToAttack)
                {
                    if (viewAbleAngle <= enemyAttack.maximumAttackAngle && viewAbleAngle >= enemyAttack.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                        {
                            return;
                        }
                        tempScore += enemyAttack.attackScore;
                        if (tempScore > randomValue)
                        {
                            currentAttack = enemyAttack;
                        }
                    }
                }
            }
        }
        private void HandleRotation(EnemyManager enemyManager)
        {
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            else
            {
                Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidBody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}
