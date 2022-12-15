using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions / Attack Actions")]
    public class EnemyAttackAction : EnemyActions
    {

        public int attackScore = 3;
        public float recoveryTime =2;

        public float maximumAttackAngle = 75;
        public float minimumAttackAngle = -75;

        public float minimumDistanceNeededToAttack = 1;
        public float MaximumDistanceNeededtoAttack = 1.5f;

    }
}

