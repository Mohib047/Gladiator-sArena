using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class PlayerAttaker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        public string lastAttack;
        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleCombos(WeaponItem weapon) 
        {
            if (inputHandler.combo) 
            {
                animatorHandler.anim.SetBool("canDoCombo", true);
                if (lastAttack == weapon.OHLightAttack1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OHLightAttack2, true);
                }
                
            }
        }

        public void HandleLightAttack(WeaponItem weapon) 
        {
            animatorHandler.PlayTargetAnimation(weapon.OHLightAttack1, true);
            lastAttack = weapon.OHLightAttack1;
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.OHHeavyAttack1, true);
            lastAttack = weapon.OHHeavyAttack1;
        }
    }
}

