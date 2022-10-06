using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class EliminatedPopUpTests
{
    private PlayerHUDComponent hudComponent;
    private GameObject eliminatedUI = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/YouDied.prefab");

    [SetUp]
    public void Setup()
    {
        GameObject gameObject = new GameObject("HUD");
        hudComponent = gameObject.AddComponent<PlayerHUDComponent>();
        hudComponent.GetType().GetField("_eliminatedPrefab", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(hudComponent, eliminatedUI);
    }


    [Test]
    public void TestPopUpSpawnsOnHealthZero()
    {
        // Run code
        hudComponent.ShowEliminatedUI();

        // Check if spawned
        Assert.NotNull(GameObject.Find($"{eliminatedUI.name}(Clone)"));
    }

    [UnityTest]
    public IEnumerator TestPopUpDisconnectToDisableGameObjectAfterFewSeconds()
    {
        // Run code
        hudComponent.ShowEliminatedUI();
        GameObject.Find($"{eliminatedUI.name}(Clone)").GetComponent<YouDiedUI>().Disconnect();

        // Wait
        yield return new WaitForSeconds(3);

        // Check if spawned
        Assert.Null(GameObject.Find($"{eliminatedUI.name}(Clone)"));
    }
}
