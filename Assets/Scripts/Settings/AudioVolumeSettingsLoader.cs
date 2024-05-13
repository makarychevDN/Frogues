using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FroguesFramework
{
    public class AudioVolumeSettingsLoader : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private List<string> volumeParameters;

        private void Start()
        {
            foreach (var parameter in volumeParameters)
            {
                if (PlayerPrefs.HasKey(parameter))
                {
                    audioMixer.SetFloat(parameter, Mathf.Log10(PlayerPrefs.GetFloat(parameter)) * 20);
                }
            }
        }
    }
}