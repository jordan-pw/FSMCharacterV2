using UnityEngine;

namespace Entity
{
    public class IdleState : BodyState
    {
        private void Update()
        {
            Move();

            // If movement vector is not 0, it's time to move
            if (movement != Vector2.zero)
            {
                ChangeState(GetComponent<MoveState>());
            }

            // If jumping and on the ground, jump
            if (jumping && characterController.isGrounded)
            {
                ChangeState(GetComponent<JumpState>());
            }
        }

        /// <summary>
        /// Idle Move method, provides gravity and gravity only.
        /// </summary>
        protected override void Move()
        {
            // Gravity: If grounded = -1f (for character controller weirdness), and if in the air, gravity is just gravity.
            velocity.y = characterController.isGrounded ? -1f : velocity.y += gravity * Time.deltaTime;
            // Displacement is the distance the entity will move per frame
            Vector3 displacement = velocity * Time.deltaTime;

            characterController.Move(displacement);
        }
    }
}