using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class ResolutionSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionsDropDown;
        [SerializeField] private Toggle fullScreenToggle;
        private List<Resolution> _resolutions;
        private Vector2Int _currentResolution;

        private void Start()
        {
            _resolutions = Screen.resolutions.ToList();
            LoadSetup();
            RemoveDuplicateResolutions();
            SetDropDownElements();
        }

        private void RemoveDuplicateResolutions()
        {
            int resolutionsQuantity = _resolutions.Count;
            var hashedResolution = _resolutions[0];
            
            for (int i = 0; i < resolutionsQuantity; i++)
            {
                hashedResolution = _resolutions[i];

                for (int j = i + 1; j < resolutionsQuantity; j++)
                {
                    if (hashedResolution.width == _resolutions[j].width &&
                        hashedResolution.height == _resolutions[j].height)
                    {
                        _resolutions.RemoveAt(j);
                        i = 0;
                        resolutionsQuantity--;
                        break;
                    }
                }
            }
        }

        private void SetDropDownElements()
        {
            resolutionsDropDown.ClearOptions();
            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            
            for (int i = 0; i < _resolutions.Count; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);

                if (_resolutions[i].width == _currentResolution.x &&
                    _resolutions[i].height == _currentResolution.y )
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionsDropDown.AddOptions(options);
            resolutionsDropDown.value = currentResolutionIndex;
            resolutionsDropDown.RefreshShownValue();
        }

        private void LoadSetup()
        {
            fullScreenToggle.isOn = Screen.fullScreen;
            
            if (PlayerPrefs.HasKey("ScreenResolutionX"))
            {
                _currentResolution.x = PlayerPrefs.GetInt("ScreenResolutionX");
                _currentResolution.y = PlayerPrefs.GetInt("ScreenResolutionY");
                return;
            }

            _currentResolution.x = Screen.currentResolution.width;
            _currentResolution.y = Screen.currentResolution.height;
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ScreenResolutionX", resolution.width);
            PlayerPrefs.SetInt("ScreenResolutionY", resolution.height);
        }
        
        public void SetFullScreenMode(bool isFullScreen)
        {
            Screen.SetResolution(Screen.width, Screen.height, isFullScreen);
        }
    }
}