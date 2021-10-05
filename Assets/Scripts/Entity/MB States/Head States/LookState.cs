using UnityEngine;

namespace Entity
{
    public class LookState : HeadState
    {
        [SerializeField]
        private Transform tempBody; // to be replaced by an animator at some point

        private Transform inputSpace;

        private void Start()
        {
            inputSpace = Camera.main.transform;
        }

        private void Update()
        {
            Look();

            //State change behavior here
        }

        private void Look()
        {
            Vector3 rotationTarget = new Vector3(ConvertMouse().x, 0f, ConvertMouse().y);
            float rotateSpeed = 100f * Time.deltaTime;

            Quaternion rotationAngle = Quaternion.LookRotation(rotationTarget);
            rotationAngle *= Quaternion.Euler(0, inputSpace.rotation.eulerAngles.y, 0);
            tempBody.rotation = Quaternion.Lerp(tempBody.rotation, rotationAngle, rotateSpeed);
        }
    }
}