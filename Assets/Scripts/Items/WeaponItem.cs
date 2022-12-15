using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib
{
    [CreateAssetMenu (menuName = " Items/Weapon Item")]
    public class WeaponItem : Items
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("One Handed Attack Animations")]
        public string OHLightAttack1;
        public string OHLightAttack2;
        public string OHLightAttack3;
        public string OHHeavyAttack1;

        [Header("Idle animations")]
        public string rightHandIdle;
        public string leftHandIdle;
    }
    

}
