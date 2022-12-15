using mohib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public ChaseState chaseState;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            HandleRotation(enemyManager);
            if (enemyManager.isPerformingAction) 
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }
            //Check for attack range
            if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximunAttackRange) 
            {
                return attackState;
            }
            else if (distanceFromTarget > enemyManager.maximunAttackRange)
            {
                return chaseState;
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
