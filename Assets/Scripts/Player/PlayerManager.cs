using Mohib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        PlayerLocomotive playerLocomotive;
        Animator anim;
        CameraHandler cameraHandler;

        public bool isInteracting;
        public bool isSprinting;
        public bool isGrounded;
        public bool isInAir;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;

        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLocomotive = GetComponent<PlayerLocomotive>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            float delta = Time.deltaTime;
            canDoCombo = anim.GetBool("canDoCombo");
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            inputHandler.TickInput(delta);
            playerLocomotive.HandleMovement(delta);
            playerLocomotive.HandleSprint(delta);
            playerLocomotive.HandleFalling(delta, playerLocomotive.moveDirection);
        }
        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }
        private void LateUpdate()
        {
            inputHandler.sprint = false;
            isSprinting = inputHandler.bInput;
            inputHandler.rbInput = false;
            inputHandler.rtInput = false;
            if (isInAir) 
            {
                playerLocomotive.inAirTimer += Time.deltaTime;
            }
        }
    }
}

