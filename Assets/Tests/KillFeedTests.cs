using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;


public class KillFeedTests
{
    private GameObject _panel;
    private KillFeedUI _killFeedUI;
    private CanvasGroup _canvasGroup;
    private GameObject _prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/Panel - Kill Feed.prefab");

    // Gets called before every test
    [SetUp]
    public void Setup()
    {
        _panel = Object.Instantiate(_prefab);
        _killFeedUI = _panel.GetComponent<KillFeedUI>();
        _canvasGroup = _panel.GetComponent<CanvasGroup>();
    }

    // Gets called after all tests have been excecuted
    [TearDown]
    public void AfterTest()
    {
        Object.Destroy(_panel);
    }

    // Tests if message gets properly displayed.
    [UnityTest]
    public IEnumerator TestIfMessageShowsCorrectly()
    {
        // Arrange
        TMP_Text text = _killFeedUI.GetComponentInChildren<TMP_Text>();

        // Act
        _killFeedUI.ShowMessage("You killed a team");

        // Assert
        Assert.IsTrue(0f < _canvasGroup.alpha);
        Assert.AreEqual(text, _killFeedUI.Text);

        // Act
        yield return new WaitUntil(() => _canvasGroup.alpha == 0f);

        // Assert
        Assert.IsTrue(_canvasGroup.alpha == 0f);
    }
}
