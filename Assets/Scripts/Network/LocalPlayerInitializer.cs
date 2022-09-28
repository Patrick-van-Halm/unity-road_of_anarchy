using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LocalPlayerInitializer : NetworkBehaviour
{
    public UnityEvent OnIsLocalPlayer = new UnityEvent();
    [SerializeField] private List<Component> localPlayerComponents = new List<Component>();

    void Start()
    {
        if (!isLocalPlayer) return;
        OnIsLocalPlayer.Invoke();
        foreach (Component component in localPlayerComponents)
        {
            component.GetType().GetProperty("enabled")?.SetValue(component, true, null);
        }
    }
}
