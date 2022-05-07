using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace FroguesFramework
{
    public class PixelPerfectZoom : MonoBehaviour
    {
        [SerializeField] private PixelPerfectCamera pixelPerfectCamera;
        private int _defaultPixelsPerUnit = 48;

        public void UpdateResolution(float scale)
        {
            pixelPerfectCamera.assetsPPU = _defaultPixelsPerUnit * (int)scale;
        }

        private void Awake()
        {
            pixelPerfectCamera ??= FindObjectOfType<PixelPerfectCamera>();
        }
    }
}