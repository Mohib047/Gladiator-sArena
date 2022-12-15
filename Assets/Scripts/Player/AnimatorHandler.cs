using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class AnimatorHandler : CharacterAnimatorManager
    {

        PlayerManager playerManager;
        InputHandler inputHandler;
        PlayerLocomotive playerLocomotive;
        int vertical;
        int horizontal;
        public bool canRotate;

        public void Initialize() 
        {
            playerManager = GetComponent<PlayerManager>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponent<InputHandler>();
            playerLocomotive = GetComponent<PlayerLocomotive>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValue(float verticalMovement, float horizontalMovement , bool isSprinting) 
        {
            #region Vertical
            float v = 0;
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else 
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting) 
            {
                v = 2;
                h = horizontalMovement;
            }
            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }
      
        public void CanRotate() 
        {
            canRotate = true;
        }
        public void StopRotation() 
        {
            canRotate = false;
        }

        public void EnableCombo() 
        {
            anim.SetBool("canDoCombo", true);
        }
        public void DisEnableCombo() 
        {
            anim.SetBool("canDoCombo", false);
        }
        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false) 
            {
                return;
            }
            float delta = Time.deltaTime;
            playerLocomotive.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotive.rigidbody.velocity = velocity;
        }
    }

}
