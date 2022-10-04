using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using UnityEngine.TestTools;

public class PlayerAttributeTests
{
    private GameObject GameObject { get; set; }

    [SetUp]
    public void InitTests()
    {
        GameObject = new GameObject(nameof(PlayerAttributeTests));
        GameObject.AddComponent<AttributeComponent>();
    }

    [Test]
    public void WhenPlayerReceivesDamageShouldDecreaseHealth()
    {
        float damage = 10f;
        float expectedHealth = 90f;

        AttributeComponent attributeComponent = GameObject.GetComponent<AttributeComponent>();

        // Write to current health which is a syncvar and is only able to be written to as server
        attributeComponent.CurrentHealth = 100;

        // Simulate server methods
        attributeComponent.ApplyDamage(damage);

        Assert.AreEqual(expectedHealth, attributeComponent.CurrentHealth);
    }

    [Test]
    public void WhenPlayerDamageExceedsZeroHealthStaysZero()
    {
        float damage = 110f;
        float expectedHealth = 0f;

        AttributeComponent attributeComponent = GameObject.GetComponent<AttributeComponent>();

        // Write to current health which is a syncvar and is only able to be written to as server
        attributeComponent.CurrentHealth = 100;

        // Simulate server methods
        attributeComponent.ApplyDamage(damage);

        Assert.AreEqual(expectedHealth, attributeComponent.CurrentHealth);
    }

    [Test]
    public void WhenPlayerReceivesDamageOnHealthChangedIsInvoked()
    {
        float damage = 10f;
        float expectedHealth = 90f;
        float invokedValue = 0f;

        AttributeComponent attributeComponent = GameObject.GetComponent<AttributeComponent>();
        // Write to current health which is a syncvar and is only able to be written to as server
        attributeComponent.CurrentHealth = 100;
        attributeComponent.OnHealthChanged = new UnityEvent<float>();
        attributeComponent.OnHealthChanged.AddListener((float currentHealth) => invokedValue = currentHealth);

        // Simulate server methods
        attributeComponent.ApplyDamage(damage);
        attributeComponent.GetType().GetMethod("OnCurrentHealthChanged", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attributeComponent, new object[] { 100, 90 });

        Assert.AreEqual(expectedHealth, attributeComponent.CurrentHealth);
    }
}