using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerCamera
{
    [RequireComponent(typeof(PlayerInput))]
    public class CameraInput : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;

        private Queue<float> inputs = new Queue<float>();

        public void OnRotate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                float rotateDir = context.ReadValue<float>();
                inputs.Enqueue(rotateDir);
                Debug.Log("Rotation axis: " + rotateDir);
            }
        }

        /// <summary>
        /// Pops the front of the input queue and returns it
        /// </summary>
        /// <returns></returns>
        public float TakeInput()
        {
            // If the queue is not empty, dequeue and return
            if (inputs.Count > 0)
            {
                return inputs.Dequeue();
            }
            else
            {
                return 0f;
            }
        }
    }
}

