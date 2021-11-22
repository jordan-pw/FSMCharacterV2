using UnityEngine;

namespace Entity
{
    public class DashState : BodyState
    {
        [SerializeField]
        private Stamina stamina;

        private Transform inputSpace;

        private float currentStamina;
        private float dashSpeed;
        private float dashAcceleration;
        private float duration;

        private Vector3 startDir;

        protected override void OnEnable()
        {
            base.OnEnable();

            duration = definition.dashTime;
            dashSpeed = definition.dashSpeed;
            dashAcceleration = definition.dashAcceleration;

            if ((stamina.CurrentStamina - definition.dashCost) >= 0)
            {
                stamina.CurrentStamina -= definition.dashCost;
            }
            else
            {
                dashing = false;
                SetAnimator();
                ChangeState(GetComponent<MoveState>());
            }


            if (movement == Vector2.zero)
                startDir = transform.forward;
            else
            {
                startDir.x = velocity.x;
                startDir.z = velocity.y;
            }
        }
        private void Start()
        {
            inputSpace = Camera.main.transform;
        }

        private void Update()
        {
            duration -= Time.deltaTime;
            if (duration >= 0f)
            {
                Move();
                SetAnimator();
            }
            else
            {
                dashing = false;
                SetAnimator();
                ChangeState(GetComponent<MoveState>());
            }

        }

        protected override void Move()
        {
            float maxSpeedChange = dashAcceleration * Time.deltaTime;

            Vector3 desiredVelocity;

            Vector3 direction;
            if (movement == Vector2.zero)
            {
                direction = startDir;
            }
            else
            {
                direction.x = movement.x;
                direction.z = movement.y;
            }

            if (inputSpace)
            {
                desiredVelocity = inputSpace.TransformDirection(direction.x * dashSpeed, 0f, direction.z * dashSpeed);
            }
            else
                desiredVelocity = new Vector3(direction.x * dashSpeed, 0f, direction.z * dashSpeed);

            // Move towards the desired velocity at a rate of one max speed change per frame.
            velocity.x =
                Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z =
                Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

            Vector3 displacement = velocity * Time.deltaTime;
            characterController.Move(displacement);
        }
    }
}