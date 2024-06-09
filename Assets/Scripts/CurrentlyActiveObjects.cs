using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class CurrentlyActiveObjects : MonoBehaviour
    {
        private HashSet<MonoBehaviour> _activeObjects = new HashSet<MonoBehaviour>();
        private HashSet<string> _allActivatedForSessionObjects = new HashSet<string>();

        public void Add(MonoBehaviour something)
        {
            _activeObjects.Add(something);

            if (!_allActivatedForSessionObjects.Contains(something.ToString() + something.transform.root.name))
                _allActivatedForSessionObjects.Add(something.ToString() + something.transform.root.name);

        }

        public void Remove(MonoBehaviour something)
        {
            _activeObjects.Remove(something);
        }

        public void Clear() => _activeObjects.Clear();

        public bool SomethingIsActNow => _activeObjects.Count != 0;
    }
}