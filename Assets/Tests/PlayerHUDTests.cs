using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class PlayerHUDTests
{
    private GameObject HitMarkerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/HitMarker.prefab");
    private GameObject GameObject {get; set;}
    private GameObject hitmarker;

    [SetUp]
    public void InitTests()
    {
        GameObject = new GameObject(nameof(PlayerHUDTests));
        GameObject.AddComponent<PlayerHUDComponent>();

        hitmarker = Object.Instantiate(HitMarkerPrefab);
    }

    [Test]
    public void WhenEnemyIsHitHitMarkerIsEnabled()
    {
        // Get the hud component
        PlayerHUDComponent hud = GameObject.GetComponent<PlayerHUDComponent>();
        
        // Assign prefab
        hud.GetType().GetField("_hitMarker", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(hud, hitmarker);

        // Assert before callback that hitmarker is disabled
        Assert.IsFalse(hitmarker.activeSelf);

        // Invoke callback for OnEnemyHit on hud component
        hud.GetType().GetMethod("OnEnemyHit", BindingFlags.Public | BindingFlags.Instance).Invoke(hud);

        // Check if hitmarker is enabled
        Assert.IsTrue(hitmarker.activeSelf);
    }

    [UnityTest]
    public IEnumerator WhenHitMarkerTimeRunsOutHitMarkerIsDisabled()
    {
        PlayerHUDComponent hud = GameObject.GetComponent<PlayerHUDComponent>();
        
        float hitmarkerDelay = (float)hud.GetType().GetField("_hitMarkerActiveTime", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(hud);

        hud.GetType().GetMethod("OnEnemyHit", BindingFlags.Public | BindingFlags.Instance).Invoke(hud);

        yield return new WaitForSeconds(hitmarkerDelay);

        Assert.IsFalse(hitmarker.activeSelf);
    }
}
