using UnityEngine;
using UnityEngine.UI;


namespace FroguesFramework
{
    public class MaterialInstanceContainer : MonoBehaviour
    {
        [SerializeField] private Material material;
        
        private void Awake()
        {
            if(GetComponent<Image>() != null)
                material = GetComponent<Image>().material;
            
            if(GetComponent<SpriteRenderer>() != null)
                material = GetComponent<SpriteRenderer>().material;
        }
    }
}