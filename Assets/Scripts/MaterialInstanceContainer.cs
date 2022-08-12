using UnityEngine;
using UnityEngine.UI;


namespace FroguesFramework
{
    public class MaterialInstanceContainer : Container<Material>
    {
        private void Awake()
        {
            if(GetComponent<Image>() != null)
                Content = GetComponent<Image>().material;
            
            if(GetComponent<SpriteRenderer>() != null)
                Content = GetComponent<SpriteRenderer>().material;
        }
    }
}