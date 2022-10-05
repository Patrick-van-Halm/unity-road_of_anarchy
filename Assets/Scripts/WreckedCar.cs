using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WreckedCar : MonoBehaviour
{
    [SerializeField] private GameObject _wreckedBuggy;

    public GameObject WreckedBuggy { get { return _wreckedBuggy; } }
    private Transform _position;

    public UnityEvent<string> KillFeed;

    private AttributeComponent _vehicleAttributes;

    private void Awake()
    {
        _vehicleAttributes = GetComponent<AttributeComponent>();
    }

    private void Start()
    {
        _vehicleAttributes.OnHealthChanged.AddListener(CheckIfDestroyed);
    }

    private void CheckIfDestroyed(float health)
    {
        if (health <= 0f) PlaceWreckedCar();
    }

    public void PlaceWreckedCar()
    {
        KillFeed?.Invoke("You killed a team");

        _position = GetComponent<Transform>();
        Destroy(this.gameObject);
        _wreckedBuggy = Instantiate(_wreckedBuggy, _position.position, _position.rotation);
    }
}
