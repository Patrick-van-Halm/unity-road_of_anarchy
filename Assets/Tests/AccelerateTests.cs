using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class AccelerateTests
{
    private GameObject _prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Models/Car.prefab");

    private GameObject _gameObject;
    private Car _car;
    private CarInput _carInput;
    private WheelCollider _wheel;

    // Gets called before all tests get excecuted
    [SetUp]
    public void Setup()
    {
        _gameObject = Object.Instantiate(_prefab);
        _car = _gameObject.GetComponent<Car>();
        _carInput = _gameObject.GetComponent<CarInput>();
        _wheel = _gameObject.GetComponentInChildren<WheelCollider>();
    }

    // Gets called after all tests have been excecuted
    [TearDown]
    public void AfterTest()
    {
        Object.Destroy(_gameObject);
    }

    // Tests if the wheel on the car has a brakeTorque = 0 and motorTorque > 0 when accelerating
    [Test]
    public void TestIfCarAccelerates()
    {
        // Arrange


        // Act
        _carInput.Accelerating = true;
        _car.Accelerate();

        // Assert
        Assert.AreEqual(_wheel.brakeTorque, 0f);
        Assert.IsTrue(_wheel.motorTorque > 0);
    }

    // Tests if the wheel on the car has a motorTorque = 0 and brakeTorque > 0 when decelerating
    [Test]
    public void TestIfCarDecelerates()
    {
        // Arrange

        // Act
        _carInput.Accelerating = false;
        _car.Decelerate();

        // Assert
        Assert.AreEqual(_wheel.motorTorque, 0f);
        Assert.IsTrue(_wheel.brakeTorque > 0);
    }

    // Tests if the wheel on the car has a motorTorque = 0 and brakeTorque > 0 when braking
    [Test]
    public void TestIfCarBrakes()
    {
        // Arrange
        _car.CurrentSpeed = 3f;

        //Act
        _carInput.Braking = true;
        _car.Brake();

        // Assert
        Assert.AreEqual(_wheel.motorTorque, 0f);
        Assert.IsTrue(_wheel.brakeTorque > 0);
    }

    // Tests if the wheel on the car has a motorTorque < 0 and brakeTorque = 0 when reversing
    [Test]
    public void TestIfCarReverses()
    {
        // Arrange
        _car.CurrentSpeed = 1f;

        //Act
        _carInput.Braking = true;
        _car.Brake();

        // Assert
        Assert.AreEqual(_wheel.brakeTorque, 0f);
        Assert.IsTrue(_wheel.motorTorque < 0);
    }
}
