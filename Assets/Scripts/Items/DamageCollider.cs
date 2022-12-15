using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;
        public int currentWeaponDamage = 25;
        public AudioSource slashSource;
        public AudioClip slash;
        public ParticleSystem iceEffect;
        public ParticleSystem bloodEffect;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnabledDamageCollider()
        {
            damageCollider.enabled = true;

        }
        public void DisEnabledDamageCollider() 
        {
            damageCollider.enabled = false;

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") 
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
                if(playerStats != null) 
                {
                    slashSource.PlayOneShot(slash);
                    Instantiate(iceEffect, damageCollider.transform.position, damageCollider.transform.rotation);
                    playerStats.TakeDamage(currentWeaponDamage);
                    Instantiate(bloodEffect, playerStats.transform.position, playerStats.transform.rotation);
                }
            }
            if (other.tag == "Enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    slashSource.PlayOneShot(slash);
                    Instantiate(iceEffect,damageCollider.transform.position, damageCollider.transform.rotation);
                    enemyStats.TakeDamage(currentWeaponDamage);
                    Instantiate(bloodEffect, enemyStats.transform.position, enemyStats.transform.rotation);

                }

            }
        }
    }

}

