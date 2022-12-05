using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActivator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _list1 = new();
    [SerializeField] private List<GameObject> _list2 = new();
    private bool _activeList1 = false;

    private void Start()
    {
        Switch();
    }

    public void Switch()
    {
        List<GameObject> activateList = _list1;
        List<GameObject> deactivateList = _list1;
        if (_activeList1)
        {
            activateList = _list2;
            deactivateList = _list1;
        }
        _activeList1 = !_activeList1;

        foreach (GameObject go in deactivateList) go.SetActive(false);
        foreach (GameObject go in activateList) go.SetActive(true);
    }
}
