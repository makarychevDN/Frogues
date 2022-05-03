using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace FroguesFramework
{
    public class PixelPerfectZoom : MonoBehaviour
    {
        [SerializeField] private PixelPerfectCamera pixelPerfectCamera;
        private int[] _xResolutions = {336, 420, 560, 840, 2200};

        public void UpdateResolution(float value)
        {
            pixelPerfectCamera.refResolutionX = _xResolutions[(int) value];
        }

        private void Awake()
        {
            pixelPerfectCamera ??= FindObjectOfType<PixelPerfectCamera>();
        }
    }
}