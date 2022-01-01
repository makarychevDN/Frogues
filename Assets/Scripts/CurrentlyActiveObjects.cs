using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentlyActiveObjects
{
    public static HashSet<MonoBehaviour> activeObjects = new HashSet<MonoBehaviour>();
    public static HashSet<string> AllActivatedForSessionObjects = new HashSet<string>();

    public static void Add(MonoBehaviour something)
    {
        activeObjects.Add(something);

        if (!AllActivatedForSessionObjects.Contains(something.ToString() + something.transform.root.name))
            AllActivatedForSessionObjects.Add(something.ToString() + something.transform.root.name);
        
    }

    public static void Remove(MonoBehaviour something)
    {
        activeObjects.Remove(something);
    }

    public static bool SomethingIsActNow => activeObjects.Count != 0;
}
