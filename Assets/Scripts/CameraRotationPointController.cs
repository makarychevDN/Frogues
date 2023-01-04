using UnityEngine;

namespace FroguesFramework
{
    public class CameraRotationPointController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.Mouse2))
            {
                transform.Rotate(transform.up, Input.GetAxis("Mouse X"));
            }
        }
    }
}