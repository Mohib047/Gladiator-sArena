using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        public bool bInput;
        public bool rbInput;
        public bool rtInput;
        public bool lockOnInput;
        public float bInputTimer;


        public bool sprint;
        public bool combo;
        public bool lockOn;



        PlayerControls inputActions;
        CameraHandler cameraHandler;
        PlayerAttaker playerAttaker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        AnimatorHandler animatorHandler;


        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttaker = GetComponent<PlayerAttaker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponent<AnimatorHandler>();
        }
        public void OnEnable()
        {
            if (inputActions == null) 
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => rbInput = true;
                inputActions.PlayerActions.RT.performed += i => rtInput = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }
        public void TickInput(float delta) 
        {
            MoveInput(delta);
            HandleSprint(delta);
            HandleAttackInput(delta);
            HandleLockOn();
        }
        private void MoveInput(float delta) 
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
        private void HandleSprint(float delta) 
        {
            bInput = inputActions.PlayerActions.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
            if (bInput)
            {
                bInputTimer += delta;
                sprint = true;
            }
            else 
            {
                if (bInputTimer > 0 && bInputTimer < 0.5f) 
                {
                    sprint = false;
                }
                bInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta) 
        {
            
            //For Right Hand
            if (rbInput) 
            {
                if (playerManager.canDoCombo)
                {
                    combo = true;
                    playerAttaker.HandleCombos(playerInventory.rightWeapon);
                    combo = false;
                }
                else 
                {
                    if (playerManager.canDoCombo) 
                    {
                        return;
                    }
                    playerAttaker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }
            if (rtInput)
            {
                if (playerManager.canDoCombo)
                {
                    return;
                }
                animatorHandler.anim.SetBool("isUsingRightHand", true);
                playerAttaker.HandleHeavyAttack(playerInventory.rightWeapon);

            }
        }

        private void HandleLockOn() 
        {
            if (lockOnInput && !lockOn)
            {
                cameraHandler.ClearLockOnTarget();
                lockOnInput = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null) 
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOn = true;
                }
            }
            else if (lockOnInput && lockOn) 
            {
                lockOnInput = false;
                lockOn = false;
                cameraHandler.ClearLockOnTarget();
            }
        }
        
    }
}

