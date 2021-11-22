using UnityEngine;

namespace Entity
{
    public abstract class BodyState : MonoBehaviour
    {
        /*
         * TODO: In the future, this will be a humanoid subclass of some basic entity body state
         */

        // All states require these components. Be mindful when using them.
        [SerializeField]
        protected CharacterController characterController;
        [SerializeField]
        protected EntityManager manager;
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected HumanoidDefinition definition;

        // Fields required for movement physics. When events are from the manager, these are set.
        protected Vector3 velocity;
        protected Vector2 movement;
        protected bool jumping, sprinting, crouching, dashing;
        // Gravity multiplied by some multiplier, for snappier jumps
        protected float gravity = -9.81f * 2;

        // Entity stats required for movement
        protected int maxAcceleration;
        protected int maxAirAcceleration;
        protected int baseSpeed, sprintSpeed, crouchSpeed;
        protected int jumpHeight;

        private RaycastHit hit;

        protected virtual void OnEnable()
        {
            // Subscribe to all movement events
            manager.SendMove += OnMove;
            manager.SendJump += OnJump;
            manager.SendSprint += OnSprint;
            manager.SendCrouch += OnCrouch;
            manager.SendDash += OnDash;

        }

        protected virtual void OnDisable()
        {
            /* 
             * Unsubscribe to all movement events
             * If not done, states all try to do things simulataneously
             */
            manager.SendMove -= OnMove;
            manager.SendJump -= OnJump;
            manager.SendSprint -= OnSprint;
            manager.SendCrouch -= OnCrouch;
            manager.SendDash -= OnDash;
        }

        protected void SetAnimator()
        {
            // Ignoring y component
            Vector3 flatForward = transform.forward;
            Vector3 flatRight = transform.right;
            flatForward.y = 0f;
            flatRight.y = 0f;
            Vector3 flatVelocity = velocity;
            flatVelocity.y = 0f;

            float velocityZ = Vector3.Dot(flatVelocity.normalized, flatForward);
            float velocityX = Vector3.Dot(flatVelocity.normalized, flatRight);

            //Debug.Log("Velocity Z: " + velocityZ + "Velocity X: " + velocityX);

            animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
            animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

            animator.SetBool("Crouching", crouching);
            animator.SetBool("Sprinting", sprinting);
            animator.SetBool("Dodge", dashing);

            animator.SetFloat("DodgeTime", definition.dashTime);

            if (!characterController.isGrounded)
            {
                if (velocity.y > 0f)
                {
                    animator.SetBool("Jumping", true);
                }
                else if(velocity.y <= -2f)
                {
                    animator.SetBool("Jumping", false);
                    animator.SetBool("Falling", true);
                }
            }
            else
            {
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", false);
            }

        }

        protected void ChangeState(BodyState newState)
        {
            // New state needs to know what happened while it was disabled
            newState.SendStats(velocity, movement, jumping, sprinting, crouching, dashing);
            // Then, once it recieves info, enable it, and disable the current state
            this.enabled = false;
            newState.enabled = true;
        }

        /// <summary>
        /// All states require some logic for movement, whether it be simple gravity, or complex sprinting and rolling
        /// </summary>
        protected abstract void Move();

        protected void SendStats(Vector3 vel, Vector2 mov, bool jump, bool sprint, bool crouch, bool dash)
        {
            velocity = vel;
            movement = mov;
            jumping = jump;
            sprinting = sprint;
            crouching = crouch;
            dashing = dash;
        }

        #region Event Actions
        /*
         * Once these methods are invoked, fields will be set to cooresponding values from the manager.
         * This dictates the fundamental state conditions
         */
        protected virtual void OnMove(Vector2 move)
        {
            movement = move;
        }

        protected virtual void OnJump(bool jump)
        {
            jumping = characterController.isGrounded ? jump : false;
        }

        protected virtual void OnSprint(bool sprint)
        {
            sprinting = sprint;
        }

        protected virtual void OnCrouch(bool crouch)
        {
            crouching = crouch;
        }

        protected virtual void OnDash(bool dash)
        {
            dashing = dash;
        }
        #endregion

        #region Slope Functions

        protected bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 * 1.5f))
                if (hit.normal != Vector3.up)
                    return true;
            return false;
        }

        protected bool OnSteepSlope()
        {
            if (!characterController.isGrounded) return false;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 + 1.5f))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle > characterController.slopeLimit) return true;
            }
            return false;
        }

        protected Vector3 GetSlopeNormal()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 * 1.5f))
                if (hit.normal != Vector3.up)
                    return hit.normal;
            return Vector3.one;
        }

        protected Vector3 GetSlopePoint()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 * 1.5f))
                if (hit.normal != Vector3.up)
                    return hit.point;
            return Vector3.one;
        }
        #endregion

        private void Awake()
        {
            // Stats come from the definition, must be declared
            maxAcceleration = definition.maxAcceleration.BaseValue;
            maxAirAcceleration = definition.maxAirAcceleration.BaseValue;

            baseSpeed = definition.speed.BaseValue;
            sprintSpeed = definition.sprintSpeed.BaseValue;
            crouchSpeed = definition.crouchSpeed.BaseValue;

            jumpHeight = definition.jumpHeight.BaseValue;
        }
    }
}

