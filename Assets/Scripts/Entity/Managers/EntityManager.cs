using UnityEngine;

namespace Entity
{
    public abstract class EntityManager : MonoBehaviour
    {
        // Delegate and event for movement vector2, invoked when the entity wants to move
        public delegate void Move(Vector2 move);
        public event Move SendMove;
        public event Move SendLook;

        // Delegate and event for boolean movement actions
        public delegate void MoveAction(bool mAction);
        public event MoveAction SendJump;
        public event MoveAction SendSprint;
        public event MoveAction SendCrouch;
        public event MoveAction SendDash;

        // These fields represent how an entity is trying to move
        protected Vector2 movement;
        protected bool sprinting = false;
        protected bool crouching = false;
        protected bool jump = false;
        protected bool dash = false;

        #region Events
        /**
         * Due to the nature of this system, in order for events to still work for subclasses
         * we must invoke our events here, in some wrapper method.
         * Then, subclasses can invoke events by calling these wrapper methods.
         */
        protected virtual void RaiseSendMove(Vector2 move)
        {
            SendMove?.Invoke(move);
        }

        protected virtual void RaiseSendJump(bool mAction)
        {
            SendJump?.Invoke(mAction);
        }

        protected virtual void RaiseSendSprint(bool mAction)
        {
            SendSprint?.Invoke(mAction);
        }

        protected virtual void RaiseSendCrouch(bool mAction)
        {
            SendCrouch?.Invoke(mAction);
        }

        protected virtual void RaiseSendDash(bool mAction)
        {
            SendDash?.Invoke(mAction);
        }

        protected virtual void RaiseSendLook(Vector2 look)
        {
            SendLook?.Invoke(look);
        }
        #endregion
    }
}
