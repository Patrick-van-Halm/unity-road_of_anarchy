using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class WheelsGroundedTests
{
    private GameObject _playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Models/RobbertsTestPlayer.prefab");

    private GameObject _playerGameObject;
    private CarInfo _carInfo;
    private NewKartScript _kartScript;

    [SetUp]
    public void Setup()
    {
        _playerGameObject = Object.Instantiate(_playerPrefab);
        _carInfo = _playerGameObject.GetComponent<CarInfo>();
        _kartScript = _playerGameObject.GetComponent<NewKartScript>();
    }

    [TearDown]
    public void AfterTest()
    {
        Object.Destroy(_playerGameObject);
    }

    [Test]
    public void AllWheelsGroundedAndParametersFalseFalse_ReturnsFalse()
    {
        // Arrange
        bool _expectedBoolean = false;
        _kartScript.GetType().GetProperty("GroundPercent", BindingFlags.Public | BindingFlags.Instance).SetValue(_kartScript, 1.0f);

        // Act
        bool _actualBoolean = (bool)_carInfo.GetType().GetMethod("WheelsGrounded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carInfo, new object[] { false, false });

        // Assert
        Assert.AreEqual(_expectedBoolean, _actualBoolean);
    }

    [Test]
    public void AllWheelsGroundedAndParametersFalseTrue_ReturnsFalse()
    {
        // Arrange
        bool _expectedBoolean = false;
        _kartScript.GetType().GetProperty("GroundPercent", BindingFlags.Public | BindingFlags.Instance).SetValue(_kartScript, 1.0f);

        // Act
        bool _actualBoolean = (bool)_carInfo.GetType().GetMethod("WheelsGrounded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carInfo, new object[] { false, true });

        // Assert
        Assert.AreEqual(_expectedBoolean, _actualBoolean);
    }

    [Test]
    public void AllWheelsGroundedAndParametersTrueFalse_ReturnsTrue()
    {
        // Arrange
        bool _expectedBoolean = true;
        _kartScript.GetType().GetProperty("GroundPercent", BindingFlags.Public | BindingFlags.Instance).SetValue(_kartScript, 1.0f);

        // Act
        bool _actualBoolean = (bool)_carInfo.GetType().GetMethod("WheelsGrounded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carInfo, new object[] { true, false });

        // Assert
        Assert.AreEqual(_expectedBoolean, _actualBoolean);
    }

    [Test]
    public void NoWheelsGroundedAndParametersFalseFalse_ReturnsTrue()
    {
        // Arrange
        bool _expectedBoolean = true;
        _kartScript.GetType().GetProperty("GroundPercent", BindingFlags.Public | BindingFlags.Instance).SetValue(_kartScript, 0.0f);

        // Act
        bool _actualBoolean = (bool)_carInfo.GetType().GetMethod("WheelsGrounded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carInfo, new object[] { false, false });

        // Assert
        Assert.AreEqual(_expectedBoolean, _actualBoolean);
    }

    [Test]
    public void NoWheelsGroundedAndParametersFalseTrue_ReturnsFalse()
    {
        // Arrange
        bool _expectedBoolean = false;
        _kartScript.GetType().GetProperty("GroundPercent", BindingFlags.Public | BindingFlags.Instance).SetValue(_kartScript, 0.0f);

        // Act
        bool _actualBoolean = (bool)_carInfo.GetType().GetMethod("WheelsGrounded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carInfo, new object[] { false, true });

        // Assert
        Assert.AreEqual(_expectedBoolean, _actualBoolean);
    }

    [Test]
    public void NoWheelsGroundedAndParametersTrueFalse_ReturnsFalse()
    {
        // Arrange
        bool _expectedBoolean = false;
        _kartScript.GetType().GetProperty("GroundPercent", BindingFlags.Public | BindingFlags.Instance).SetValue(_kartScript, 0.0f);

        // Act
        bool _actualBoolean = (bool)_carInfo.GetType().GetMethod("WheelsGrounded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carInfo, new object[] { true, false });

        // Assert
        Assert.AreEqual(_expectedBoolean, _actualBoolean);
    }

    [Test]
    public void SomeWheelsGroundedAndParametersFalseFalse_ReturnsFalse()
    {
        // Arrange
        bool _expectedBoolean = false;
        _kartScript.GetType().GetProperty("GroundPercent", BindingFlags.Public | BindingFlags.Instance).SetValue(_kartScript, 0.5f);

        // Act
        bool _actualBoolean = (bool)_carInfo.GetType().GetMethod("WheelsGrounded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carInfo, new object[] { false, false });

        // Assert
        Assert.AreEqual(_expectedBoolean, _actualBoolean);
    }

    [Test]
    public void SomeWheelsGroundedAndParametersFalseTrue_ReturnsTrue()
    {
        // Arrange
        bool _expectedBoolean = true;
        _kartScript.GetType().GetProperty("GroundPercent", BindingFlags.Public | BindingFlags.Instance).SetValue(_kartScript, 0.5f);

        // Act
        bool _actualBoolean = (bool)_carInfo.GetType().GetMethod("WheelsGrounded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carInfo, new object[] { false, true });

        // Assert
        Assert.AreEqual(_expectedBoolean, _actualBoolean);
    }

    [Test]
    public void SomeWheelsGroundedAndParametersTrueFalse_ReturnsFalse()
    {
        // Arrange
        bool _expectedBoolean = false;
        _kartScript.GetType().GetProperty("GroundPercent", BindingFlags.Public | BindingFlags.Instance).SetValue(_kartScript, 0.5f);

        // Act
        bool _actualBoolean = (bool)_carInfo.GetType().GetMethod("WheelsGrounded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carInfo, new object[] { true, false });

        // Assert
        Assert.AreEqual(_expectedBoolean, _actualBoolean);
    }
}
