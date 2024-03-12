using UnityEngine;
using UnityEngine.Audio;

namespace FroguesFramework
{
    public class AudioVolumeSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        
        public void SetMasterVolume(float value) => SetVolume("MasterVolume", value);
        public void SetMusicVolume(float value) => SetVolume("MusicVolume", value);
        public void SetSoundsVolume(float value) => SetVolume("SoundsVolume", value);
        public void SetUIVolume(float value) => SetVolume("UIVolume", value);

        private void SetVolume(string exposedParameterName, float value) => audioMixer.SetFloat(exposedParameterName, Mathf.Log10(value) * 20);
    }
}