using UnityEngine;

namespace PlayerCamera
{
    [RequireComponent(typeof(Camera))]
    public class OrbitCamera : MonoBehaviour
    {
        [SerializeField]
        private CameraInput cameraInput;

        [SerializeField]
        private Transform focus = default;

        // Distance from the focus
        [SerializeField, Range(20f, 200f)]
        private float distance = 5f;

        // Radius the camera can stray from focus
        [SerializeField, Min(0f)]
        private float focusRadius = 1f;

        // Rate in which the camera aims at focus
        [SerializeField, Range(0f, 1f)]
        private float focusCentering = 0.5f;

        // Speed in which the camera rotates
        [SerializeField, Range(1f, 360f)]
        private float rotationSpeed = 90f;

        // Fixed rotation will snap the camera when rotation
        [SerializeField]
        private bool fixedRotation = false;
        // Rotation angle is the degree in which the camera the snaps
        [SerializeField, Range(1f, 90f)]
        private float rotationAngle = 45f;
        // input is the current value of the input axis
        private float input = 0;

        private Vector3 focusPoint;

        private Vector2 orbitAngles = new Vector2(45f, 0f);

        private bool goalReached = true;

        float goalAngle = 0f;

        private void Awake()
        {
            focusPoint = focus.position;
        }

        private void LateUpdate()
        {
            // Update where the focus is
            UpdateFocusPosition();
            // Enable the player to control rotation
            ManualRotation();

            // Math for setting camera position and rotation
            Quaternion lookRotation = Quaternion.Euler(orbitAngles);
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = focusPoint - lookDirection * distance;
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        private void UpdateFocusPosition()
        {
            Vector3 targetPoint = focus.position;
            // If the camera is allowed to stray from the focus, do stuff
            if (focusRadius > 0f)
            {
                //Distance from the focus position to where the current focus is
                float distance = Vector3.Distance(targetPoint, focusPoint);
                float t = 1f;
                if (distance > 0.01f && focusCentering > 0f)
                {
                    t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
                }
                if (distance > focusRadius)
                {
                    t = Mathf.Min(t, focusRadius / distance);
                }
                // Lerp the focus to the target at rate t
                focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
            }
            else
            {
                focusPoint = targetPoint;
            }
        }

        private void ManualRotation()
        {
            // Input is the 1D axis from the camera input

            const float e = 0.001f;
            const float angleThreshold = 5f;
            float angleDistance = Mathf.Abs(goalAngle - orbitAngles.y);

            if (input < e || input > e)
            {
                // store input into a variable
                float storedInput = input;
                // fixed rotation = snappy rotation
                if (fixedRotation)
                {
                    // If the camera is at the desired angle, the goal angle can be set to something new, based on input
                    if (goalReached)
                    {
                        input = cameraInput.TakeInput();
                        goalAngle = orbitAngles.y + rotationAngle * storedInput;
                        if (goalAngle != orbitAngles.y) goalReached = false;
                    }
                    // If the camera is not at the desired angle, smoothly move towards it
                    if (goalAngle != orbitAngles.y)
                    {
                        // If we are close to goal angle, but not quite there, check if there are more queued inputs
                        if (angleDistance <= angleThreshold)
                        {
                            input = cameraInput.TakeInput();
                            goalAngle = goalAngle + rotationAngle * -input;
                        }

                        // Smoothly move towards the goal angle
                        orbitAngles.y = Mathf.SmoothStep(orbitAngles.y, goalAngle, rotationSpeed * Time.unscaledDeltaTime);
                        // Set goalReached to true once the goal angle is reached
                        if (goalAngle == orbitAngles.y) goalReached = true;
                    }

                }
                else // If not fixedRotation, allow free rotation
                {
                    orbitAngles.y += rotationSpeed * Time.unscaledDeltaTime * input;
                }
            }
        }
    }
}