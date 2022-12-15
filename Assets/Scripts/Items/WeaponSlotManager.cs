using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        DamageCollider leftDamageCollider;
        DamageCollider rightDamageCollider;

        PlayerManager playerManager;

        Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerManager = GetComponent<PlayerManager>();
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots) 
            {
                if (weaponSlot.isLeftHandSlot) 
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isleft) 
        {
            if (isleft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.leftHandIdle, 0.2f);
                }
                else 
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
            }
            else 
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();

                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.rightHandIdle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Tight Arm Empty", 0.2f);
                }
            }
        }
        #region Damage Colliders
        private void LoadLeftWeaponDamageCollider() 
        {
            leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        private void LoadRightWeaponDamageCollider() 
        {
            rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        }
        public void OpenDamageCollider() 
        {
            if (playerManager.isUsingRightHand)
            {
                rightDamageCollider.EnabledDamageCollider();
            }
            else 
            {
                leftDamageCollider.EnabledDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            rightDamageCollider.DisEnabledDamageCollider();
            leftDamageCollider.DisEnabledDamageCollider();
        }
        
        #endregion
    }

}