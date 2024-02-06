using UnityEngine;

public class ReplaceAnimatorByKeyPressed : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController controller;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.runtimeAnimatorController = controller;
        }
    }
}
