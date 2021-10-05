using UnityEngine;

namespace Entity
{
    public class MoveState : BodyState
    {
        private Transform inputSpace;

        private void Start()
        {
            inputSpace = Camera.main.transform;
        }

        private void Update()
        {
            Move();

            // If there is no velocity for the x and z directions (the ground), then switch to idle
            if (velocity.x == 0f && velocity.z == 0f)
            {
                ChangeState(GetComponent<IdleState>());
            }

            // If jumping and on the ground, jump
            if (jumping && characterController.isGrounded)
            {
                ChangeState(GetComponent<JumpState>());
            }
        }

        /// <summary>
        /// Moves the character controller on the x and z planes, at different rates
        /// Rate depends on sprinting, crouching, whether or not the entity is grounded
        /// </summary>
        protected override void Move()
        {
            /*
             * If sprinting, speed = sprintSpeed
             * if not, speed = baseSpeed
             * Check for crouching after, since crouching will override sprinting.
             */
            int speed = sprinting ? sprintSpeed : baseSpeed;
            speed = crouching ? crouchSpeed : speed;

            // Max Speed Change = the max change in speed per frame. Varies depending on if in the air or not
            float maxSpeedChange = characterController.isGrounded ?
                maxAcceleration * Time.deltaTime : maxAirAcceleration * Time.deltaTime;

            // Desired velocity is the max possible velocity
            Vector3 desiredVelocity;

            if (inputSpace)
            {
                desiredVelocity =
                    inputSpace.TransformDirection(movement.x * speed, 0f, movement.y * speed);
            }
            else
                desiredVelocity = new Vector3(movement.x * speed, 0f, movement.y * speed);


            // Move towards the desired velocity at a rate of one max speed change per frame.
            velocity.x =
                Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z =
                Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
            // Gravity: If grounded = -1f (for character controller weirdness), and if in the air, gravity is just gravity.
            velocity.y = characterController.isGrounded ? -1f : velocity.y += gravity * Time.deltaTime;

            // Displacement is the distance the entity will move per frame
            Vector3 displacement = velocity * Time.deltaTime;

            SetAnimator();
            characterController.Move(displacement);
        }
    }
}
