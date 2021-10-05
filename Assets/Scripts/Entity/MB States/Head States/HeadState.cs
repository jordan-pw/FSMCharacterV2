using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class HeadState : MonoBehaviour
    {
        [SerializeField]
        protected CharacterController characterController;
        [SerializeField]
        protected EntityManager manager;
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected HumanoidDefinition definition;

        protected Camera mainCamera;

        protected Vector2 lookDirection;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        protected virtual void OnEnable()
        {
            // Subscribe to all movement events
            manager.SendLook += OnLook;
        }

        protected virtual void OnDisable()
        {
            /* 
             * Unsubscribe to all movement events
             * If not done, states all try to do things simulataneously
             */
            manager.SendLook -= OnLook;
        }

        protected void ChangeState(HeadState newState)
        {
            // New state needs to know what happened while it was disabled
            newState.SendStats(lookDirection);
            // Then, once it recieves info, enable it, and disable the current state
            newState.enabled = true;
            this.enabled = false;
        }

        protected void SendStats(Vector2 look)
        {
            lookDirection = look;
        }

        protected Vector3 ConvertMouse()
        {
            Vector3 mousePosition = mainCamera.ScreenToViewportPoint(lookDirection);
            Vector2 relativeMouseVector = (mousePosition - new Vector3(0.5f, 0.5f, 0.0f)).normalized;
            return relativeMouseVector;
        }

        #region Event Actions
        /*
         * Once these methods are invoked, fields will be set to cooresponding values from the manager.
         * This dictates the fundamental state conditions
         */
        protected virtual void OnLook(Vector2 look)
        {
            lookDirection = look;
        }
        #endregion
    }
}