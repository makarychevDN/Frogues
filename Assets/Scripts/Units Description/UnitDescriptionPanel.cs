using FroguesFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitDescriptionPanel : MonoBehaviour, IAbleToDisablePreVisualization
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text textField;

        public void Activate(string text)
        {
            panel.SetActive(true);
            textField.text = text;
        }

        public void DisablePreVisualization()
        {
            panel.SetActive(false);
        }
    }
}