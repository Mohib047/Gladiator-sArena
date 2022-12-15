using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class EnemyAnimatorManager : CharacterAnimatorManager
    {
        EnemyManager enemyManager;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponent<EnemyManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.velocity = velocity;
        }
    }
}

