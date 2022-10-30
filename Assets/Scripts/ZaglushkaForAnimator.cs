using UnityEngine;

namespace FroguesFramework
{
    public class ZaglushkaForAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                _animator.SetTrigger("Take Damage");
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                _animator.SetTrigger("Kick");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _animator.SetInteger("Weapon Index", 0);
            }            
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _animator.SetInteger("Weapon Index", 1);
            }
        }
    }
}