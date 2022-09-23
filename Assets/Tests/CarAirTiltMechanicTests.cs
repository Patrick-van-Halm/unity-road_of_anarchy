using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class CarAirTiltMechanicTests
{
    private GameObject _playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/RobbertsTestPlayer.prefab");

    private GameObject _playerGameObject;
    private Rigidbody _playerRB;
    private CarInput _carInput;
    private CarAirTiltMechanic _carAirTiltMechanic;

    [SetUp]
    public void Setup()
    {
        _playerGameObject = Object.Instantiate(_playerPrefab);
        _playerRB = _playerGameObject.GetComponent<Rigidbody>();
        _carInput = _playerGameObject.GetComponent<CarInput>();
        _carAirTiltMechanic = _playerGameObject.GetComponent<CarAirTiltMechanic>();
    }

    [TearDown]
    public void AfterTest()
    {
        Object.Destroy(_playerGameObject);
    }

    [Test]
    public void TestCarFallingSfxHandler_OnGroundAndLastOnGroundSameValue()
    {
        // Arrange
        float _expectedValueOfHighestRawVolume = 20;
        _carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, _expectedValueOfHighestRawVolume);
        _carAirTiltMechanic.GetType().GetField("_onGround", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, true);
        _carAirTiltMechanic.GetType().GetField("_lastOnGround", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, true);

        // Act
        _carAirTiltMechanic.GetType().GetMethod("CarFallingSfxHandler", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carAirTiltMechanic, new object[] { });
        float _actualValueOfHighestRawVolume = (float)_carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedValueOfHighestRawVolume, _actualValueOfHighestRawVolume);
    }

    [Test]
    public void TestCarFallingSfxHandler_OnGroundTrueAndLastOnGroundFalse()
    {
        // Arrange
        float _expectedValueOfHighestRawVolume = 20;
        bool _expectedValueOfLastOnGround = true;
        _carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, _expectedValueOfHighestRawVolume);
        _carAirTiltMechanic.GetType().GetField("_onGround", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, true);
        _carAirTiltMechanic.GetType().GetField("_lastOnGround", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, false);

        // Act
        _carAirTiltMechanic.GetType().GetMethod("CarFallingSfxHandler", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carAirTiltMechanic, new object[] { });
        float _actualValueOfHighestRawVolume = (float)_carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);
        bool _actualValueOfLastOnGround = (bool)_carAirTiltMechanic.GetType().GetField("_lastOnGround", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedValueOfHighestRawVolume, _actualValueOfHighestRawVolume);
        Assert.AreEqual(_expectedValueOfLastOnGround, _actualValueOfLastOnGround);
    }

    [Test]
    public void TestCarFallingSfxHandler_OnGroundFalseAndLastOnGroundTrue()
    {
        // Arrange
        float _expectedValueOfHighestRawVolume = 20;
        bool _expectedValueOfLastOnGround = false;
        _carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, _expectedValueOfHighestRawVolume);
        _carAirTiltMechanic.GetType().GetField("_onGround", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, false);
        _carAirTiltMechanic.GetType().GetField("_lastOnGround", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, true);

        // Act
        _carAirTiltMechanic.GetType().GetMethod("CarFallingSfxHandler", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carAirTiltMechanic, new object[] { });
        float _actualValueOfHighestRawVolume = (float)_carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);
        bool _actualValueOfLastOnGround = (bool)_carAirTiltMechanic.GetType().GetField("_lastOnGround", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);

        // Assert
        Assert.AreNotEqual(_expectedValueOfHighestRawVolume, _actualValueOfHighestRawVolume);
        Assert.AreEqual(_expectedValueOfLastOnGround, _actualValueOfLastOnGround);
    }

    [Test]
    public void TestCarSfxVolumeHandler_HighestRawVolumeLowerThanVelocity()
    {
        // Arrange
        float _expectedNewValueOfHighestRawVolume = 30;
        float _currentValueOfHighestRawVolume = 10;
        _carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, _currentValueOfHighestRawVolume);
        _playerRB.velocity = new Vector3(0, _expectedNewValueOfHighestRawVolume, 0);

        // Act
        _carAirTiltMechanic.GetType().GetMethod("CarSfxVolumeHandler", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carAirTiltMechanic, new object[] { });
        float _actualNewValueOfHighestRawVolume = (float)_carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedNewValueOfHighestRawVolume, _actualNewValueOfHighestRawVolume);
        Assert.AreNotEqual(_currentValueOfHighestRawVolume, _actualNewValueOfHighestRawVolume);
    }

    [Test]
    public void TestCarSfxVolumeHandler_HighestRawVolumeHigherThanVelocity()
    {
        // Arrange
        float _expectedValueOfHighestRawVolume = 30;
        float _velocity = 20;
        _carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, _expectedValueOfHighestRawVolume);
        _playerRB.velocity = new Vector3(0, _velocity, 0);

        // Act
        _carAirTiltMechanic.GetType().GetMethod("CarSfxVolumeHandler", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carAirTiltMechanic, new object[] { });
        float _actualValueOfHighestRawVolume = (float)_carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedValueOfHighestRawVolume, _actualValueOfHighestRawVolume);
        Assert.AreNotEqual(_velocity, _actualValueOfHighestRawVolume);
    }

    [Test]
    public void TestCarSfxVolumeHandler_VelocityLowerThanMax()
    {
        // Arrange
        float _expectedValueOfCarSfxVolume = 0.5f;
        float _maxRawVolume = (float)_carAirTiltMechanic.GetType().GetField("_maxRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);
        float _halveOfMaxRawVolume = _maxRawVolume / 2;
        _carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, _halveOfMaxRawVolume);

        // Act
        _carAirTiltMechanic.GetType().GetMethod("CarSfxVolumeHandler", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carAirTiltMechanic, new object[] { });
        float _actualValueOfCarSfxVolume = (float)_carAirTiltMechanic.GetType().GetField("_actualVolume", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedValueOfCarSfxVolume, _actualValueOfCarSfxVolume);
    }

    [Test]
    public void TestCarSfxVolumeHandler_VelocityHigherThanMax()
    {
        // Arrange
        float _expectedValueOfCarSfxVolume = 1;
        float _maxRawVolume = (float)_carAirTiltMechanic.GetType().GetField("_maxRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);
        float _currentValueOfHighestRawVolume = _maxRawVolume + 5f;
        _carAirTiltMechanic.GetType().GetField("_highestRawVolume", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_carAirTiltMechanic, _currentValueOfHighestRawVolume);

        // Act
        _carAirTiltMechanic.GetType().GetMethod("CarSfxVolumeHandler", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_carAirTiltMechanic, new object[] { });
        float _actualValueOfCarSfxVolume = (float)_carAirTiltMechanic.GetType().GetField("_actualVolume", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_carAirTiltMechanic);

        // Assert
        Assert.AreEqual(_expectedValueOfCarSfxVolume, _actualValueOfCarSfxVolume);
    }
}
