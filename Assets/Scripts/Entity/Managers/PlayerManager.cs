using UnityEngine;
using UnityEngine.InputSystem;

namespace Entity.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerManager : EntityManager
    {
        [SerializeField]
        private PlayerInput playerInput;

        /* 
         * Set to true or false when crouching and sprinting are desired to be
         * turned on or off by key presses, rather than on while pressing
         */
        [SerializeField]
        private bool toggleCrouch, toggleSprint;

        #region Unity Input Events
        /**
        * Unity Input Events
        * Invoked by some key press, defined in the PlayerInput component
        * These all invoke their respective RaiseSendX() events from the base class
        * As well as doing certain manipulations to the movement fields before invoking
        */
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 vec = context.ReadValue<Vector2>();

            movement.x = vec.x;
            movement.y = vec.y;

            RaiseSendMove(movement);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed) jump = true;
            if (context.canceled) jump = false;

            RaiseSendJump(jump);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (toggleSprint)
            {
                sprinting = !sprinting;
            }
            else
            {
                if (context.performed) sprinting = true;
                if (context.canceled) sprinting = false;
            }
            RaiseSendSprint(sprinting);
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (toggleCrouch)
            {
                crouching = !crouching;
            }
            else
            {
                if (context.performed) crouching = true;
                if (context.canceled) crouching = false;
            }
            RaiseSendCrouch(crouching);
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (context.performed) dash = true;
            if (context.canceled) dash = false;

            RaiseSendDash(dash);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Vector2 vec = context.ReadValue<Vector2>();

            // At all times, the look direction must be a normalized vector2
            if (playerInput.currentControlScheme == "Keyboard") // If keyboard input, special math must be done to normalize
            {
                // Normalize(vec);
                RaiseSendLook(vec);
            }
            else // If controller input, the look direction is already normalized
            {
               // vec;
            }
        }
        #endregion
    }
}