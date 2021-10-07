using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(CharacterController))]
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
            float velocityZ = Vector3.Dot(velocity.normalized, transform.forward);
            float velocityX = Vector3.Dot(velocity.normalized, transform.right);

            Debug.Log("Velocity Z: " + velocityZ + "Velocity X: " + velocityX);

            animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
            animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

            animator.SetBool("Crouching", crouching);
            animator.SetBool("Sprinting", sprinting);

            if (!characterController.isGrounded)
            {
                if (velocity.y > 0)
                {
                    animator.SetBool("Jumping", true);
                }
                else
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
            Debug.Log("Changing from: " + this + " to : " + newState);
            // New state needs to know what happened while it was disabled
            newState.SendStats(velocity, movement, jumping, sprinting, crouching);
            // Then, once it recieves info, enable it, and disable the current state
            newState.enabled = true;
            this.enabled = false;
        }

        /// <summary>
        /// All states require some logic for movement, whether it be simple gravity, or complex sprinting and rolling
        /// </summary>
        protected abstract void Move();

        protected void SendStats(Vector3 vel, Vector2 mov, bool jump, bool sprint, bool crouch)
        {
            velocity = vel;
            movement = mov;
            jumping = jump;
            sprinting = sprint;
            crouching = crouch;
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

