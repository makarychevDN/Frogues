using System.Collections;
using System.Collections.Generic;
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