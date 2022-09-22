using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInitializer : NetworkBehaviour
{
    [SerializeField] private List<Component> localPlayerComponents = new List<Component>();

    void Start()
    {
        if (!isLocalPlayer) return;
        foreach(Component component in localPlayerComponents)
        {
            component.GetType().GetProperty("enabled")?.SetValue(component, true, null);
        }
    }
}
