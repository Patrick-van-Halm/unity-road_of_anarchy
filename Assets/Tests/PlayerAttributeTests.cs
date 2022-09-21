using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class PlayerAttributeTests
{
    private GameObject GameObject {get; set;}

    [SetUp]
    public void InitTests()
    {
        GameObject = new GameObject(nameof(PlayerAttributeComponent));
        GameObject.AddComponent<PlayerAttributeComponent>();
    }

    [Test]
    public void WhenPlayerReceivesDamageShouldDecreaseHealth()
    {
        float damage = 10f;
        float expectedHealth = 90f;

        PlayerAttributeComponent attributeComponent = GameObject.GetComponent<PlayerAttributeComponent>();
        attributeComponent.ApplyDamage(damage);

        Assert.AreEqual(expectedHealth, attributeComponent.CurrentHealth);
    }

    [Test]
    public void WhenPlayerDamageExceedsZeroHealthStaysZero()
    {
        float damage = 110f;
        float expectedHealth = 0f;
        
        PlayerAttributeComponent attributeComponent = GameObject.GetComponent<PlayerAttributeComponent>();
        attributeComponent.ApplyDamage(damage);

        Assert.AreEqual(expectedHealth, attributeComponent.CurrentHealth);
    }

    [Test]
    public void WhenPlayerReceivesDamageOnHealthChangedIsInvoked()
    {
        float damage = 10f;
        float expectedHealth = 90f;
        float invokedValue = 0f;

        PlayerAttributeComponent attributeComponent = GameObject.GetComponent<PlayerAttributeComponent>();
        attributeComponent.OnHealthChanged = new UnityEvent<float>();
        attributeComponent.OnHealthChanged.AddListener((float currentHealth) => invokedValue = currentHealth);
        attributeComponent.ApplyDamage(damage);

        Assert.AreEqual(expectedHealth, attributeComponent.CurrentHealth);
    }
}