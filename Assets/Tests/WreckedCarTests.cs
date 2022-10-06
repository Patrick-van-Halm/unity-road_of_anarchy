using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class WreckedCarTests
{
    private GameObject _buggyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Buggy.prefab");

    private GameObject _car;
    private WreckedCar _wreckedCar;

    // Gets called before every test
    [SetUp]
    public void Setup()
    {
        _car = Object.Instantiate(_buggyPrefab);
        _wreckedCar = _car.GetComponent<WreckedCar>();
    }

    // Gets called after all tests have been excecuted
    [TearDown]
    public void AfterTest()
    {
        Object.Destroy(_car);

    }

    // Tests if wrecked car is instantiated and checks if positions are correct
    [Test]
    public void TestIfWreckedCarSuccessfullyInstantiates()
    {
        // Arrange

        // Act
        _wreckedCar.PlaceWreckedCarWhenHealthZero(0);

        // Assert
        Assert.IsTrue(_wreckedCar.WreckedBuggy.activeInHierarchy);
        Assert.AreEqual(_wreckedCar.WreckedBuggy.gameObject.transform.position, _car.transform.position);
    }
}
