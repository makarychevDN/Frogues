using System;
using UnityEngine;
using UnityEngine.UI;


namespace FroguesFramework
{
    public class MaterialInstanceContainer : MonoBehaviour
    {
        public Material materialPrefab;
        private Material _materialInstance;
        public Color _color;
        public Material MaterialPrefab => _materialInstance;
        
        private void Start()
        {
            _materialInstance = new Material(materialPrefab);
            GetComponent<Image>().material = _materialInstance;
        }

        private void Update()
        {
            _materialInstance.color = _color;
        }
    }
}