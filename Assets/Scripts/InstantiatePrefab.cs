using UnityEngine;
namespace FroguesFramework
{
    public class InstantiatePrefab : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        public void Run()
        {
            Instantiate(prefab);
        }
    }
}
