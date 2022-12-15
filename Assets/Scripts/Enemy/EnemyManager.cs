using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Mohib 
{
    public class EnemyManager :CharacterManager
    {
        public State currentState;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyStats enemyStats;

        public bool isPerformingAction;
        public bool isInteracting;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public CharacterStats currentTarget;

        public float detectionRadius = 20;
        public float maximumDetectionAngle = 75;
        public float minimumDetectionAngle = -75;

        public float rotationSpeed = 25;
        public float maximunAttackRange = 1.5f;
        public float currentRecoveryTime = 0;

        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidBody;

        private void Awake()
        {
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidBody = GetComponent<Rigidbody>();

        }
        private void Start()
        {
            navMeshAgent.enabled = false;
            enemyRigidBody.isKinematic = false;
        }
        private void Update()
        {
            HandleRecoverytimer();
            isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");

        }

        private void FixedUpdate()
        {
            HandleStateMachine();

        }
        private void HandleStateMachine() 
        {
            if (currentState != null) 
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);
                if (nextState != null) 
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State nextState) 
        {
            currentState = nextState;
        }

        private void HandleRecoverytimer() 
        {
            if (currentRecoveryTime > 0) 
            {
                currentRecoveryTime -= Time.deltaTime;
            }
            if (isPerformingAction) 
            {
                if (currentRecoveryTime <= 0) 
                {
                    isPerformingAction = false;
                }
            }
        }

    }
}

