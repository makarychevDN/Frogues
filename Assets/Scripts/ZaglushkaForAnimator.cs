using UnityEngine;

namespace FroguesFramework
{
    public class ZaglushkaForAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                _animator.SetTrigger("Take Damage");
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                _animator.SetTrigger("Kick");
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                _animator.SetTrigger("Attack");
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                _animator.SetTrigger("Death");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _animator.SetInteger("Weapon Index", 0);
            }            
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _animator.SetInteger("Weapon Index", 1);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _animator.SetInteger("Weapon Index", 2);
            }
        }
    }
}