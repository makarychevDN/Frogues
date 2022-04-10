using UnityEngine;

namespace FroguesFramework
{
    public abstract class CurrentlyActiveBehaviour : MonoBehaviour
    {
        protected bool ActiveNow
        {
            set
            {
                if (value)
                    CurrentlyActiveObjects.Add(this);
                else
                    CurrentlyActiveObjects.Remove(this);
            }
        }
    }
}