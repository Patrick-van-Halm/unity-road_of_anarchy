using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerAttributeTests
{
    [Test]
    public void WhenAddingPlayerScriptShouldAutomaticallyAddComponents()
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<Player>();
        
        Player player = gameObject.GetComponent<Player>();
        PlayerAttributeComponent attributeComponent = gameObject.GetComponent<PlayerAttributeComponent>();

        Assert.NotNull(player);
        Assert.NotNull(attributeComponent);
    }
}
