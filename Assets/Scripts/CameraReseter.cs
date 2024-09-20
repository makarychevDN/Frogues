using UnityEngine;

namespace FroguesFramework
{
    public class CameraReseter : MonoBehaviour
    {
        public void ResetCamera()
        {
            FindObjectOfType<CameraController>().ResetCamera();
        }
    }
}