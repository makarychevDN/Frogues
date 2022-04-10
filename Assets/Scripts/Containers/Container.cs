using UnityEngine;

namespace FroguesFramework
{
    public class Container<T> : MonoBehaviour
    {
        [SerializeField] protected T content;

        public virtual T Content
        {
            get => content;
            set => content = value;
        }

        public virtual bool IsEmpty => Content == null;
    }
}