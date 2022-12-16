using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mohib 
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        private LayerMask ignoreLayers;
        public static CameraHandler singleton;

        public float lookspeed = 0.01f;
        public float followSpeed = 0.01f;
        public float pivotSpeed = 0.03f;

        private Vector3 offset = new Vector3(0, 0, 0.01f);
        private float targetPosition;
        private float defaultPosition;
        private float loookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public Transform currentLockOnTarget;
        public Transform nearestLockOnTarget;
        public float maxLockOnDistance = 30;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        InputHandler inputHandler;

        private void Awake()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget(float delta) 
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position,ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition - offset;
            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput) 
        {
            if (inputHandler.lockOn == false && currentLockOnTarget == null)
            {
                loookAngle += (mouseXInput * lookspeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = loookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else 
            {
                Vector3 dir = currentLockOnTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;
                dir = currentLockOnTarget.position - cameraPivotTransform.position;
                dir.Normalize();
                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        private void HandleCameraCollision(float delta) 
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransformPosition - cameraPivotTransform.position;
            direction.Normalize();
            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }
            if (Mathf.Abs(targetPosition) < minimumCollisionOffset) 
            {
                targetPosition = -minimumCollisionOffset;
            }
            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition , delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn() 
        {
            float shortestDistance = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);
            for (int i = 0; i < colliders.Length; i++) 
            {
                CharacterManager character= colliders[i].GetComponent<CharacterManager>();
                if (character != null) 
                {
                    Vector3 lockTargerDir = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position , character.transform.position);
                    float viewAbleAngle = Vector3.Angle(lockTargerDir, cameraTransform.forward);

                    if (character.transform.root != targetTransform.transform.root && viewAbleAngle > -50 && viewAbleAngle < 50 && distanceFromTarget <= maxLockOnDistance) 
                    {
                        availableTargets.Add(character);
                    }
                }
            }
            for (int j = 0; j < availableTargets.Count; j++) 
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[j].transform.position);
                if (distanceFromTarget < shortestDistance) 
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[j].lockOnTransform;
                }
            }
        }
        public void ClearLockOnTarget()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }
    }
}
