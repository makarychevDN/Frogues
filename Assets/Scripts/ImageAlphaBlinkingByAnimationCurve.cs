using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    [RequireComponent(typeof(Image))]
    public class ImageAlphaBlinkingByAnimationCurve : MonoBehaviour
    {
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private float strenghtMultiplier = 1;
        [SerializeField] private float speedMultiplier = 0.75f;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();   
        }

        private void Update()
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, animationCurve.Evaluate(Time.time * speedMultiplier) * strenghtMultiplier);
        }
    }
}