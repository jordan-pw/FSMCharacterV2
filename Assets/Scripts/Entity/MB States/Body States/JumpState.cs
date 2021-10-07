using UnityEngine;

namespace Entity
{
    public class JumpState : BodyState
    {
        protected override void OnEnable()
        {
            // Still need to subscribe to events
            base.OnEnable();
            // Jump when enabled, and only when enabled
            Jump();
        }

        private void Update()
        {
            Move();

            // If movement, then switch to move state for mid-air direction changes
            if (movement != Vector2.zero)
            {
                ChangeState(GetComponent<MoveState>());
            }

            // If the entity is grounded, then it is not jumping, and must change states
            if (characterController.isGrounded)
            {
                ChangeState(GetComponent<MoveState>());
            }
        }

        /// <summary>
        /// Applies an impulse to the y velocity, to send the entity flying into the air
        /// </summary>
        protected void Jump()
        {
            velocity.y = 0;
            // This formula determines the y velocity to impulse to reach the set jump height
            velocity.y += (Mathf.Sqrt(-2f * gravity * jumpHeight)); // -2f * gravity * jump height
            SetAnimator();
        }

        /// <summary>
        /// Applies gravity and slowly sets the x and z velocity to 0
        /// </summary>
        protected override void Move()
        {
            // Max Speed Change = the max change in speed per frame
            float maxSpeedChange = maxAirAcceleration * Time.deltaTime;

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Max(velocity.y, gravity);

            // Move the x and y velocities towards 0
            velocity.x =
                Mathf.MoveTowards(velocity.x, 0, maxSpeedChange);
            velocity.z =
                Mathf.MoveTowards(velocity.z, 0, maxSpeedChange);

            // Displacement is the distance the entity will move per frame
            Vector3 displacement = velocity * Time.deltaTime;

            SetAnimator();
            characterController.Move(displacement);
        }

    }
}
