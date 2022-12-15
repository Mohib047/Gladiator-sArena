using Mohib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace mohib 
{
    public class ChaseState : State
    {
        public CombatStanceState combatStanceState;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isPerformingAction) 
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
            float viewAbleAngle = Vector3.Angle(targetDirection, transform.forward);

            if (distanceFromTarget > enemyManager.maximunAttackRange)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }
            HandleRotation(enemyManager);
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (distanceFromTarget <= enemyManager.maximunAttackRange) 
            {
                return combatStanceState;
            }
            else 
            {
                return this; 
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

