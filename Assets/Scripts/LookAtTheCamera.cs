using UnityEngine;

namespace FroguesFramework
{
    public class LookAtTheCamera : MonoBehaviour
    {
        [SerializeField] private Transform shadow;
        [SerializeField] private bool blockXRotation = false;

        void Update()
        {
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

            if (blockXRotation)
            {
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }


            if(shadow != null)
                shadow.localEulerAngles = new Vector3(90 - transform.localEulerAngles.x, 0, 0);
        }

    }
}