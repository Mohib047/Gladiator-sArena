using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class LevelCompleteCollision : MonoBehaviour
    {
        public GameManager gameManager;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                gameManager.LevelComplete();
                Destroy(gameObject);
            }
        }
    }
}

