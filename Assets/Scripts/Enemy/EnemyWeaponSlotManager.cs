using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem LeftHandWeapon;

        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot leftHandSlot;

        DamageCollider leftHandCollider;
        DamageCollider rightHandCollider;

        private void Awake()
        {
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
        private void Start()
        {
            LoadWeaponsOnBothHands();
        }
        public void LoadWeaponsOnBothHands() 
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponSlot(rightHandWeapon, false);
            }
            if (LeftHandWeapon != null)
            {
                LoadWeaponSlot(LeftHandWeapon, true);
            }
        }
        public void LoadWeaponSlot(WeaponItem weapon, bool isleft) 
        {
            if (isleft)
            {
                leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);

            }
            else 
            {
                rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(false);
            }
        }
        public void LoadWeaponsDamageCollider(bool isleft) 
        {
            if (isleft)
            {
                leftHandCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else 
            {
                rightHandCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightHandCollider.currentWeaponDamage = 10;

            }
        }

        public void OpenDamageCollider() 
        {
            rightHandCollider.EnabledDamageCollider();
        }
        public void CloseDamageCollider() 
        {
            rightHandCollider.DisEnabledDamageCollider();
        }
    }
}
