using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohib 
{
    public class PlayerLocomotive : MonoBehaviour
    {

        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;
        PlayerManager playerManager;


        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground and air Detection stats")]
        [SerializeField]
        float groundDetectionRayStart = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToBeginFal = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreforGroundCheck;
        public float inAirTimer;

        [Header("Stats")]
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallSpeed = 45;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponent<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();
            playerManager.isGrounded = true;
            ignoreforGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandleRotation(float delta) 
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero) 
            {
                targetDir = myTransform.forward;
            }

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;

        }
        public void HandleMovement(float delta) 
        {
            if (playerManager.isInteracting) 
            {
                return;
            }
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;
            float speed = movementSpeed;
            if (inputHandler.sprint)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else 
            {
                moveDirection *= speed;

            }
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;
            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
            animatorHandler.UpdateAnimatorValue(inputHandler.moveAmount, 0, playerManager.isSprinting);
        }
        public void HandleSprint(float delta) 
        {
            if (animatorHandler.anim.GetBool("isInteracting"))
            {
                return;
            }
            if (inputHandler.sprint) 
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                if (inputHandler.moveAmount > 0) 
                {
                    animatorHandler.PlayTargetAnimation("Sprint", true);
                    moveDirection.y = 0;
                    Quaternion sprintRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = sprintRotation;
                }
            }
        }
        public void HandleFalling(float detla, Vector3 moveDirection) 
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStart;
            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f)) 
            {
                moveDirection = Vector3.zero;
            }
            if (playerManager.isInAir) 
            {
                rigidbody.AddForce(-Vector3.up * fallSpeed);
                rigidbody.AddForce(moveDirection * fallSpeed / 5f);
            }
            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFal, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFal, ignoreforGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in air fgor " + inAirTimer);
                        animatorHandler.PlayTargetAnimation("Land", true);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }
                    playerManager.isInAir = false;
                }
            }
            else 
            {
                if (playerManager.isGrounded) 
                {
                    playerManager.isGrounded = false;
                }
                if (playerManager.isInAir == false) 
                {
                    if (playerManager.isInteracting == false) 
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
;                   }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 10f);
                    playerManager.isInAir = true;
                }
            }
            if (playerManager.isGrounded) 
            {
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                }
                else 
                {
                    myTransform.position = targetPosition;
                }
            }
        }
        #endregion
    }

}

