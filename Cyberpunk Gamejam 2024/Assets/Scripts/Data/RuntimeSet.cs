using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RuntimeSet : ScriptableObject
{
    public List<GameObject> Set = new List<GameObject>();

    public void Register(GameObject entry)
    {
        if (!Set.Contains(entry))
        {
            Set.Add(entry);
        }
    }

    public void Unregister(GameObject entry)
    {
        if(Set.Contains(entry))
        {
            Set.Remove(entry);
        }
    }
}
