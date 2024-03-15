using UnityEngine;

namespace FroguesFramework
{
    public class LookAtTheCamera : MonoBehaviour
    {
        [SerializeField] private Transform shadow;

        void Update()
        {
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            shadow.localEulerAngles = new Vector3(90 - transform.localEulerAngles.x, 0, 0);
        }

    }
}