using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSingletons : MonoBehaviour
{
    [SerializeField] List<GameObject> PrefabsToDestroy = new();
    private void Awake()
    {
        foreach(GameObject prefab in PrefabsToDestroy)
        {
            foreach (GameObject instance in FindObjectsOfType<GameObject>(true))
            {
                if (prefab.name != instance.name || prefab.GetComponents<Component>().Length != instance.GetComponents<Component>().Length) continue;
                Destroy(instance);
            }
        }
    }
}
