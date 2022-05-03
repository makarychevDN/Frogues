using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class ResolutionSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionsDropDown;
        private List<Resolution> _resolutions;

        private void Start()
        {
            _resolutions = Screen.resolutions.ToList();
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
            
            resolutionsDropDown.ClearOptions();

            List<string> options = new List<string>();


            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Count; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);

                if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height )
                {
                    currentResolutionIndex = i;
                }
            }
            
            resolutionsDropDown.AddOptions(options);
            resolutionsDropDown.value = currentResolutionIndex;
            resolutionsDropDown.RefreshShownValue();
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        
        public void SetFullScreenMode(bool isFullScreen)
        {
            Screen.SetResolution(Screen.width, Screen.height, isFullScreen);
        }
    }
}