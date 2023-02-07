using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class ProcessInputTests
{
    private GameObject _playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Models/RobbertsTestPlayer.prefab");

    private GameObject _playerGameObject;
    private KeyboardInput _playerInput;
    private CarTiltMechanic _carTiltMechanic;

    [SetUp]
    public void Setup()
    {
        _playerGameObject = Object.Instantiate(_playerPrefab);
        _playerInput = _playerGameObject.GetComponent<KeyboardInput>();
        _playerInput.InputAllowed();
        _carTiltMechanic = _playerGameObject.GetComponent<CarTiltMechanic>();
    }

    [TearDown]
    public void AfterTest()
    {
        Object.Destroy(_playerGameObject);
    }

    [Test]
    public void PlayerPressedForwardKey_TiltDirectionPlus1by0()
    {
        // Arrange
        Vector2 _expectedTiltDirection = new Vector2(1, 0);
        _playerInput.GetType().GetField("_accelerating", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_playerInput, true);

        // Act
        _carTiltMechanic.GetType().GetMethod("ProcessInput", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carTiltMechanic, new object[] { });
        Vector2 _actualTiltDirection = (Vector2)_carTiltMechanic.GetType().GetField("_tiltDirection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedTiltDirection, _actualTiltDirection);
    }

    [Test]
    public void PlayerPressedBackwardKey_TiltDirectionMinus1by0()
    {
        // Arrange
        Vector2 _expectedTiltDirection = new Vector2(-1, 0);
        _playerInput.GetType().GetField("_braking", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_playerInput, true);

        // Act
        _carTiltMechanic.GetType().GetMethod("ProcessInput", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carTiltMechanic, new object[] { });
        Vector2 _actualTiltDirection = (Vector2)_carTiltMechanic.GetType().GetField("_tiltDirection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedTiltDirection, _actualTiltDirection);
    }

    [Test]
    public void PlayerPressedLeftwardKey_TiltDirection0byPlus1()
    {
        // Arrange
        Vector2 _expectedTiltDirection = new Vector2(0, 1);
        _playerInput.GetType().GetField("_steerInput", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_playerInput, -1);

        // Act
        _carTiltMechanic.GetType().GetMethod("ProcessInput", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carTiltMechanic, new object[] { });
        Vector2 _actualTiltDirection = (Vector2)_carTiltMechanic.GetType().GetField("_tiltDirection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedTiltDirection, _actualTiltDirection);
    }

    [Test]
    public void PlayerPressedRightwardKey_TiltDirection0byMinus1()
    {
        // Arrange
        Vector2 _expectedTiltDirection = new Vector2(0, -1);
        _playerInput.GetType().GetField("_steerInput", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_playerInput, 1);

        // Act
        _carTiltMechanic.GetType().GetMethod("ProcessInput", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carTiltMechanic, new object[] { });
        Vector2 _actualTiltDirection = (Vector2)_carTiltMechanic.GetType().GetField("_tiltDirection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedTiltDirection, _actualTiltDirection);
    }

    [Test]
    public void PlayerPressedBothBackwardForwardKey_TiltDirection0by0()
    {
        // Arrange
        Vector2 _expectedTiltDirection = new Vector2(0, 0);
        _playerInput.GetType().GetField("_accelerating", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_playerInput, true);
        _playerInput.GetType().GetField("_braking", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_playerInput, true);

        // Act
        _carTiltMechanic.GetType().GetMethod("ProcessInput", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carTiltMechanic, new object[] { });
        Vector2 _actualTiltDirection = (Vector2)_carTiltMechanic.GetType().GetField("_tiltDirection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedTiltDirection, _actualTiltDirection);
    }

    [Test]
    public void PlayerPressedBothLeftwardRightwardKey_TiltDirection0by0()
    {
        // Arrange
        Vector2 _expectedTiltDirection = new Vector2(0, 0);
        _playerInput.GetType().GetField("_steerInput", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_playerInput, 0);

        // Act
        _carTiltMechanic.GetType().GetMethod("ProcessInput", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carTiltMechanic, new object[] { });
        Vector2 _actualTiltDirection = (Vector2)_carTiltMechanic.GetType().GetField("_tiltDirection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedTiltDirection, _actualTiltDirection);
    }

    [Test]
    public void PlayerPressedBothForwardLeftwardKey_TiltDirection1by1Normalized()
    {
        // Arrange
        Vector2 _expectedTiltDirection = new Vector2(1, 1).normalized;
        _playerInput.GetType().GetField("_accelerating", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_playerInput, true);
        _playerInput.GetType().GetField("_steerInput", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_playerInput, -1);

        // Act
        _carTiltMechanic.GetType().GetMethod("ProcessInput", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carTiltMechanic, new object[] { });
        Vector2 _actualTiltDirection = (Vector2)_carTiltMechanic.GetType().GetField("_tiltDirection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedTiltDirection, _actualTiltDirection);
    }
}
