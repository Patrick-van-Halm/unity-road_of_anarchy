using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class AccelerateTests
{
    private GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Car.prefab");
    // A Test behaves as an ordinary method
    [Test]
    public void TestIfAccerlationIncreases()
    {
        // Arrange
        Car car = prefab.GetComponent<Car>();
        CarInput carInput = prefab.GetComponent<CarInput>();

        // Act
        carInput.Accelerating = true;
        car.Accelerate();

        // Assert
        Assert.IsTrue(car.Acceleration > 0);
    }

    [Test]
    public void TestIfAccerlationDecreases()
    {
        // Arrange
        Car car = prefab.GetComponent<Car>();
        CarInput carInput = prefab.GetComponent<CarInput>();

        // Act
        carInput.Accelerating = false;
        car.Accelerate();

        // Assert
        Assert.IsTrue(car.Acceleration !> 0);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator TestIfAccerlationIncreases()
    //{
    //    // Arrange
    //    Car car = prefab.GetComponent<Car>();
    //    CarInput carInput = prefab.GetComponent<CarInput>();

    //    // Act
    //    carInput.Accelerating = true;
    //    car.Accelerate();

    //    // Assert
    //    Assert.IsTrue(car.Acceleration > 0);
    //}
}
