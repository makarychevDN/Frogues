using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class AudioVolumeSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private SerializedDictionary<Slider, string> slidersAndMixerValues;        

        private void SetVolume(string exposedParameterName, float value)
        {
            audioMixer.SetFloat(exposedParameterName, Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat(exposedParameterName, value);
        }

        private void Awake()
        {
            foreach(var sliderAndMixerValue in slidersAndMixerValues)
            {
                if (PlayerPrefs.HasKey(sliderAndMixerValue.Value))
                {
                    sliderAndMixerValue.Key.value = PlayerPrefs.GetFloat(sliderAndMixerValue.Value);
                }

                sliderAndMixerValue.Key.onValueChanged.AddListener(_ => SetVolume(sliderAndMixerValue.Value, sliderAndMixerValue.Key.value));
            }
        }
    }
}