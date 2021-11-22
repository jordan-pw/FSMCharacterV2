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

            if (dashing && characterController.isGrounded)
            {
                ChangeState(GetComponent<DashState>());
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

            // If on a slope, we must slide
            if (OnSteepSlope())
            {
                displacement = SlopeMovement() * Time.deltaTime;
            }

            SetAnimator();
            characterController.Move(displacement);
        }

        /// <summary>
        /// Idle Slope movement, when idle and on a steep slope, slide down
        /// </summary>
        private Vector3 SlopeMovement()
        {
            Vector3 slopeDirection = Vector3.up - GetSlopeNormal() * Vector3.Dot(Vector3.up, GetSlopeNormal());
            float slideSpeed = baseSpeed + Time.deltaTime;

            Vector3 slideMovement = slopeDirection * -slideSpeed;
            slideMovement.y = slideMovement.y - GetSlopePoint().y;
            return slideMovement;
        }
    }
}